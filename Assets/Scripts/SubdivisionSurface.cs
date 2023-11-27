using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SubdivisionSurface
{
    public class Vertex
    {
        public Vector3 Position { get; set; }
        public List<HalfEdge> HalfEdges { get; } = new List<HalfEdge>();
        public Vector3 NewPosition { get; set; }

        public Vertex()
        {

        }


        public Vertex(Vector3 position)
        {
            Position = position;
        }
    }

    public class HalfEdge
    {
        public Vertex StartVertex { get; set; }
        public Face Face { get; set; }
        public HalfEdge Twin { get; set; }
        public HalfEdge Next { get; set; }
        public HalfEdge Prev { get; set; }
    }

    public class Face
    {
        public List<HalfEdge> HalfEdges { get; } = new List<HalfEdge>();
        public Vector3 FacePoint { get; set; }
    }

    public class MeshData
    {

        public List<Vertex> Vertices { get; } = new List<Vertex>();
        public List<HalfEdge> HalfEdges { get; } = new List<HalfEdge>();
        public List<Face> Faces { get; } = new List<Face>();
    }

    public class CatmullClarkSubdivision
    {
        public MeshData Subdivide(MeshData inputMeshData)
        {
            MeshData subdividedMeshData = new MeshData();

            // Create face points.
            foreach (var face in inputMeshData.Faces)
            {
                Vector3 facePoint = CalculateFacePoint(face);
                subdividedMeshData.Vertices.Add(new Vertex { Position = facePoint });
                face.FacePoint = facePoint;
            }

            // Create edge points.
            foreach (var edge in inputMeshData.HalfEdges)
            {
                Vector3 edgePoint = CalculateEdgePoint(edge);
                subdividedMeshData.Vertices.Add(new Vertex { Position = edgePoint });
            }

            // Create new vertices.
            foreach (var vertex in inputMeshData.Vertices)
            {
                Vector3 newVertexPosition = CalculateNewVertexPosition(vertex);
                subdividedMeshData.Vertices.Add(new Vertex { Position = newVertexPosition });
                vertex.NewPosition = newVertexPosition;
            }

            // Create new faces.
            foreach (var face in inputMeshData.Faces)
            {
                foreach (var halfEdge in face.HalfEdges)
                {
                    var newFace = new Face();
                    subdividedMeshData.Faces.Add(newFace);

                    Vector3 vertex1 = halfEdge.StartVertex.NewPosition;
                    Vector3 vertex2 = halfEdge.Next.StartVertex.NewPosition;
                    Vector3 vertex3 = halfEdge.Face.FacePoint;

                    newFace.HalfEdges.Add(new HalfEdge { StartVertex = new Vertex(face.FacePoint), Face = newFace });
                    newFace.HalfEdges.Add(new HalfEdge { StartVertex = new Vertex(vertex2), Face = newFace });
                    newFace.HalfEdges.Add(new HalfEdge { StartVertex = new Vertex(vertex1), Face = newFace });

                    subdividedMeshData.HalfEdges.Add(newFace.HalfEdges[0]);
                    subdividedMeshData.HalfEdges.Add(newFace.HalfEdges[1]);
                    subdividedMeshData.HalfEdges.Add(newFace.HalfEdges[2]);
                }
            }

            return subdividedMeshData;
        }

        private Vector3 CalculateFacePoint(Face face)
        {
            return face.HalfEdges.Aggregate(Vector3.zero, (acc, edge) => acc + edge.StartVertex.Position) /
                   face.HalfEdges.Count;
        }

        private Vector3 CalculateEdgePoint(HalfEdge edge)
        {
            var vertex1 = edge.StartVertex.Position;
            var vertex2 = edge.Twin.StartVertex.Position;
            var vertex3 = edge.Prev.Twin.StartVertex.Position;
            var vertex4 = edge.Next.Twin.StartVertex.Position;

            return (vertex1 + vertex2 + vertex3 + vertex4) / 4.0f;
        }

        private Vector3 CalculateNewVertexPosition(Vertex vertex)
        {
            var n = vertex.HalfEdges.Count;
            var Q = vertex.HalfEdges.Aggregate(Vector3.zero, (acc, edge) => acc + edge.Face.FacePoint);
            var R = vertex.HalfEdges.Aggregate(Vector3.zero, (acc, edge) => acc + edge.StartVertex.Position);

            return (1.0f - n) * vertex.Position + Q / n + R / n;
        }
    }

    static class SubdivisionSurfaceUtility
    {
        public static Mesh SubDivideMesh(Mesh mesh)
        {
            // Create and initialize your input mesh.
            MeshData inputMeshData =ConvertUnityMeshToCustomMesh(mesh);
            // Populate the inputMesh with vertices, edges, and faces.

            CatmullClarkSubdivision subdivision = new CatmullClarkSubdivision();
            MeshData subdividedMeshData = subdivision.Subdivide(inputMeshData);

            return ConvertCustomMeshToUnityMesh(subdividedMeshData);
            // Now, 'subdividedMesh' contains the result of the Catmull-Clark subdivision.
        }


        public static MeshData ConvertUnityMeshToCustomMesh(UnityEngine.Mesh unityMesh)
        {
            MeshData customMesh = new MeshData();

            // Convert vertices
            foreach (Vector3 vertexPosition in unityMesh.vertices)
            {
                customMesh.Vertices.Add(new Vertex { Position = vertexPosition });
            }

            // Convert triangles to faces
            int[] triangles = unityMesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Face face = new Face();
                customMesh.Faces.Add(face);

                Vertex vertex1 = customMesh.Vertices[triangles[i]];
                Vertex vertex2 = customMesh.Vertices[triangles[i + 1]];
                Vertex vertex3 = customMesh.Vertices[triangles[i + 2]];

                HalfEdge edge1 = new HalfEdge { StartVertex = vertex1, Face = face };
                HalfEdge edge2 = new HalfEdge { StartVertex = vertex2, Face = face };
                HalfEdge edge3 = new HalfEdge { StartVertex = vertex3, Face = face };

                edge1.Next = edge2;
                edge2.Next = edge3;
                edge3.Next = edge1;

                face.HalfEdges.Add(edge1);
                face.HalfEdges.Add(edge2);
                face.HalfEdges.Add(edge3);

                customMesh.HalfEdges.Add(edge1);
                customMesh.HalfEdges.Add(edge2);
                customMesh.HalfEdges.Add(edge3);
            }

            // Calculate edge twins
            foreach (HalfEdge edge in customMesh.HalfEdges)
            {
                foreach (HalfEdge twinCandidate in customMesh.HalfEdges)
                {
                    if (edge != twinCandidate && edge.StartVertex.Position == twinCandidate.Face.FacePoint &&
                        twinCandidate.StartVertex.Position == edge.Face.FacePoint)
                    {
                        edge.Twin = twinCandidate;
                        twinCandidate.Twin = edge;
                    }
                }
            }

            // Calculate previous and previous half-edges
            foreach (HalfEdge edge in customMesh.HalfEdges)
            {
                HalfEdge prevEdge = edge.Face.HalfEdges.Find(he => he.Next == edge);
                edge.Prev = prevEdge;
            }

            return customMesh;
        }
        
        public static UnityEngine.Mesh ConvertCustomMeshToUnityMesh(MeshData customMesh)
        {
            UnityEngine.Mesh unityMesh = new UnityEngine.Mesh();

            // Convert vertices
            Vector3[] unityVertices = new Vector3[customMesh.Vertices.Count];
            for (int i = 0; i < customMesh.Vertices.Count; i++)
            {
                unityVertices[i] = customMesh.Vertices[i].NewPosition; // Use the new position after subdivision.
            }
            unityMesh.vertices = unityVertices;

            // Convert faces to triangles
            List<int> triangles = new List<int>();
            foreach (Face face in customMesh.Faces)
            {
                foreach (HalfEdge edge in face.HalfEdges)
                {
                    int vertexIndex = customMesh.Vertices.IndexOf(edge.StartVertex);
                    triangles.Add(vertexIndex);
                }
            }
            unityMesh.triangles = triangles.ToArray();

            unityMesh.RecalculateNormals();
            unityMesh.RecalculateBounds();

            return unityMesh;
        }
    }
    
    
}