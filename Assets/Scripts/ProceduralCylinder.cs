using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Subdiv.Demo
{
    public class ProceduralCylinder : MonoBehaviour
    {
        [SerializeField] private MeshFilter _filter;
        [SerializeField] private MeshFilter _referenceFilter;
        
        [Range(0, 4)]
        [SerializeField] private int steps;
        [SerializeField] private bool sudivide;
        [SerializeField] private bool weld;
        [SerializeField] private bool run;

        [Range(0, 0.2f)]

        [SerializeField]   private float _bevel = .1f;
        [Range(3,12)]
        [SerializeField]   private int sides = 4;
        [SerializeField]  private Vector3 size = Vector3.one;


        public void OnValidate()
        {
            // if (run)
            {
                run = false;
                // Run();
                _filter.mesh = GeneratePrimitive();


                if (weld)
                {
                    _filter.mesh =
                        SubdivisionSurface.Subdivide(
                            SubdivisionSurface.Weld(_filter.mesh, float.Epsilon, _filter.mesh.bounds.size.x),
                            0,
                            true);
                }
                if (sudivide)
                {
                    _filter.mesh =
                        SubdivisionSurface.Subdivide(
                            SubdivisionSurface.Weld(_filter.mesh, float.Epsilon, _filter.mesh.bounds.size.x),
                            steps,
                            weld);
                }
            }
        }
        public Mesh GeneratePrimitive()
        {
            // create sides
            List<Vector3> topCapFence = new List<Vector3>();
            List<Vector3> bottomCapFence = new List<Vector3>();

            List<Vector3> topFenceVertices = new List<Vector3>();
            List<Vector3> topVertices = new List<Vector3>();
            List<Vector3> bottomFenceVertices = new List<Vector3>();
            List<Vector3> bottomVertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();

            float delta = Mathf.PI*2 / sides;
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            float height = size.y/2;


            float radius = Mathf.Sqrt(2)*0.5f;
            
            for (int i = 0; i < sides; i++)
            {

                float angle = delta * (i + 0.5f);
                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);

                float bevelOffset = Mathf.Sqrt((_bevel * _bevel) * 2); // pythag;
                
                Vector3 direction = new Vector3(x, 0, z).normalized*radius;
                Vector3 innerDirection = new Vector3(x, 0, z).normalized*(radius - bevelOffset);
                    
                topCapFence.Add(innerDirection.WithY(height));
                bottomCapFence.Add(innerDirection.WithY(-height));

                topVertices.Add(direction.WithY( height));
                bottomVertices.Add(direction.WithY( -height));

                topFenceVertices.Add(direction.WithY( height - _bevel));
                bottomFenceVertices.Add(direction.WithY( -height + _bevel));
            }

            
            //side fence
            for (int i = 0; i < sides; i++)
            {
                Debug.DrawLine(topVertices[i], bottomVertices[i], Color.red, 5);
                
                Vector3 Y = bottomVertices[(i + 1) % sides];
                Vector3 Z = bottomVertices[i];
                Vector3 A = bottomFenceVertices[(i + 1) % sides];
                Vector3 B = bottomFenceVertices[i];
                Vector3 C = topFenceVertices[(i + 1) % sides];
                Vector3 D = topFenceVertices[i];
                Vector3 E = topVertices[(i + 1) % sides];
                Vector3 F = topVertices[i];

                
                AddQuad( A, B, C, D);
                AddQuad(Y, Z, A, B);
                AddQuad(C, D, E, F);
            }

            
            // caps
            for (int i = 0; i < sides; i++)
            {
                Vector3 topCenter = new Vector3(0, height, 0);
                Vector3 A = topCapFence[i];
                Vector3 B = topCapFence[(i + 1) % sides];

                AddVertex(topCenter);
                AddVertex(B);
                AddVertex(A);
                
                AddQuad(topVertices[(i+1)%sides], topVertices[i] ,B, A );

                Vector3 bottomCenter = new Vector3(0, -height, 0);
                Vector3 C = bottomCapFence[i];
                Vector3 D = bottomCapFence[(i + 1) % sides];
                
                AddVertex(bottomCenter);
                AddVertex(C);
                AddVertex(D);
                
                AddQuad(D, C, bottomVertices[(i+1)%sides], bottomVertices[i]  );

            }
            
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            Debug.Log(  mesh.vertices .Length);
            mesh.RecalculateNormals();
            return mesh;
            // create fence

            // create cap

            // create cap fence

            // weld

            void AddVertex(Vector3 position)
            {
                triangles.Add(vertices.Count);
                vertices.Add(position);
            }
            // convert to unity mesh 
            void AddQuad( Vector3 A, Vector3 B, Vector3 C, Vector3 D)
            {
                triangles.Add(vertices.Count);
                vertices.Add(D);
                triangles.Add(vertices.Count);
                vertices.Add(A);
                triangles.Add(vertices.Count);
                vertices.Add(B);

                triangles.Add(vertices.Count);
                vertices.Add(D);
                triangles.Add(vertices.Count);
                vertices.Add(C);
                triangles.Add(vertices.Count);
                vertices.Add(A);
            }
        }
        
        public class Instantiate : MonoBehaviour
        {
            IList<Edge> edgeList = new List<Edge>();
            IList<Face> faceList = new List<Face>();
            ProBuilderMesh cube;
   
   
 
            void Start()
            {  
       
                cube = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(1,1,1));
                cube.GetComponent<MeshRenderer>().sharedMaterial = BuiltinMaterials.defaultMaterial;
 
                for (int i=0; i < faceList.Count; i++)
                {
                    faceList.Add(cube.faces[i]);
       
                }
                var edgeToBevel = cube.faces[0].edges;
                var edgeToBevel1 = cube.faces[1].edges;
                var edgeToBevel2 = cube.faces[2].edges;
                var edgeToBevel3 = cube.faces[3].edges;
                var edgeToBevel4 = cube.faces[4].edges;
                var edgeToBevel5 = cube.faces[5].edges;
                Bevel.BevelEdges(cube, edgeToBevel, 0.1f);
                Bevel.BevelEdges(cube, edgeToBevel1, 0.1f);
                Bevel.BevelEdges(cube, edgeToBevel2, 0.1f);
                Bevel.BevelEdges(cube, edgeToBevel3, 0.1f);
                Bevel.BevelEdges(cube, edgeToBevel4, 0.1f);
                Bevel.BevelEdges(cube, edgeToBevel5, 0.1f);
 
            }
 
            void Update()
            {
       
            }
        }

        public void Run()
        {

            _filter.mesh =
                SubdivisionSurface.Subdivide(SubdivisionSurface.Weld(_filter.mesh, float.Epsilon, _filter.mesh.bounds.size.x),
                    steps,
                    weld);

        }
    }

}
