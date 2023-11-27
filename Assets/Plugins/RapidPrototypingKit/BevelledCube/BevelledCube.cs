using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace RapidPrototypingKit
{
    public class BevelledCube : ProceduralPrimitive
    {
        [Header("Position and Size")]
        public Vector3 _size = Vector3.one;
        public Vector3 pivot = Vector3.zero;

        [Header("Smoothing")]
        [FormerlySerializedAs("_bevel")] [SerializeField]
        public float bevel = 0.125f;
        [Range(0, 5)] [SerializeField] private int _bevelSubdivisions = 3;

        [Header("Shearing")]
        [Range(-5, 5)] [SerializeField] private float _shearX;
        [Range(-5, 5)] [SerializeField] private float _shearZ;

        private List<int> m_index_to_verts = new List<int>();
        private float m_radius;
        private int m_N_edge;

        public Vector3 size
        {
            get => _size;
            set => _size = value;
        }

        protected override void GenerateMesh()
        {

            if (bevel == 0 || _bevelSubdivisions == 0)
            {
                GenerateUnbeveled();
            }
            else
            {
                GenerateBeveled(_bevelSubdivisions, Vector3.one * 0.5f, bevel);
            }

            PostProcessVertices();

            Matrix4x4 shearMatrix = default;
            shearMatrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
            shearMatrix.SetRow(1, new Vector4(_shearZ, 1f, _shearX, 0f));
            shearMatrix.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
            shearMatrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = shearMatrix * new Vector4(_vertices[i].x, _vertices[i].y, _vertices[i].z, 1);
            }

        }


        void PostProcessVertices()
        {
            // size = Vector3.Scale(size, transform.localScale);
            // transform.localScale = Vector3.one;

            Vector3 scale = size;

            Vector3 inverseScale = Vector3.one.DivideComponentWise(scale);
            float radius = 0.5f + (bevel);

            for (int i = 0; i < _vertices.Count; i++)
            {
                Vector3 vert = _vertices[i];

                vert.x += (scale.x - radius) * Mathf.Sign(vert.x);
                vert.y += (scale.y - radius) * Mathf.Sign(vert.y);
                vert.z += (scale.z - radius) * Mathf.Sign(vert.z);

                _vertices[i] = vert.MultiplyComponentWise(inverseScale);
            }

            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = _vertices[i].MultiplyComponentWise(size * 0.5f);
            }

            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] += pivot;
            }
        }

        void AddVertex(int i, int j, int k, Vector3 pos, Vector3 base_pos)
        {
            int pidx = k * m_N_edge * m_N_edge + j * m_N_edge + i;
            if (m_index_to_verts[pidx] < 0)
            {
                int next_idx = (int)_vertices.Count;
                m_index_to_verts[pidx] = next_idx;

                Vector3 dir = pos - base_pos;

                if (dir.magnitude > 0)
                {
                    dir.Normalize();
                    _vertices.Add(base_pos + dir * m_radius);
                    _normals.Add(dir);
                }
                else
                {
                    _vertices.Add(pos);
                    _normals.Add(pos);
                }
            }
        }

        int translateIndices(int i, int j, int k)
        {
            int pidx = k * m_N_edge * m_N_edge + j * m_N_edge + i;
            return m_index_to_verts[pidx];
        }

        void GenerateBeveled(int N, Vector3 b, float radius)
        {
            m_index_to_verts.Clear();

            int N_edge = 2 * (N + 1);
            m_N_edge = N_edge;

            Resize(m_index_to_verts, N_edge * N_edge * N_edge, -1);

            m_radius = radius;
            float dx = radius / (float)N;

            float[] sign = { -1f, 1f };
            int[] ks = { 0, N * 2 + 1 };


            // xy-planes
            for (int kidx = 0; kidx < 2; ++kidx)
            {
                int k = ks[kidx];
                Vector3 origin = new Vector3(-b[0] - radius, -b[1] - radius, (b[2] + radius) * sign[kidx]);
                for (int j = 0; j <= N; ++j)
                for (int i = 0; i <= N; ++i)
                {
                    Vector3 pos = origin + new Vector3(dx * i, dx * j, 0f);
                    AddVertex(i, j, k, pos, new Vector3(-b[0], -b[1], b[2] * sign[kidx]));

                    pos = origin + new Vector3(dx * i + 2f * b[0] + radius, dx * j, 0f);
                    AddVertex(i + N + 1, j, k, pos, new Vector3(b[0], -b[1], b[2] * sign[kidx]));

                    pos = origin + new Vector3(dx * i + 2f * b[0] + radius, dx * j + 2f * b[1] + radius, 0f);
                    AddVertex(i + N + 1, j + N + 1, k, pos, new Vector3(b[0], b[1], b[2] * sign[kidx]));

                    pos = origin + new Vector3(dx * i, dx * j + 2f * b[1] + radius, 0f);
                    AddVertex(i, j + N + 1, k, pos, new Vector3(-b[0], b[1], b[2] * sign[kidx]));
                }

                // corners
                for (int j = 0; j < N; ++j)
                {
                    for (int i = 0; i < N; ++i)
                    {
                        AddTriangle(translateIndices(i, j, k), translateIndices(i + 1, j + 1, k),
                            translateIndices(i, j + 1, k), kidx == 0);
                        AddTriangle(translateIndices(i, j, k), translateIndices(i + 1, j, k),
                            translateIndices(i + 1, j + 1, k), kidx == 0);

                        AddTriangle(translateIndices(i, j + N + 1, k), translateIndices(i + 1, j + N + 2, k),
                            translateIndices(i, j + N + 2, k), kidx == 0);
                        AddTriangle(translateIndices(i, j + N + 1, k), translateIndices(i + 1, j + N + 1, k),
                            translateIndices(i + 1, j + N + 2, k), kidx == 0);

                        AddTriangle(translateIndices(i + N + 1, j + N + 1, k),
                            translateIndices(i + N + 2, j + N + 2, k), translateIndices(i + N + 1, j + N + 2, k),
                            kidx == 0);
                        AddTriangle(translateIndices(i + N + 1, j + N + 1, k),
                            translateIndices(i + N + 2, j + N + 1, k), translateIndices(i + N + 2, j + N + 2, k),
                            kidx == 0);

                        AddTriangle(translateIndices(i + N + 1, j, k), translateIndices(i + N + 2, j + 1, k),
                            translateIndices(i + N + 1, j + 1, k), kidx == 0);
                        AddTriangle(translateIndices(i + N + 1, j, k), translateIndices(i + N + 2, j, k),
                            translateIndices(i + N + 2, j + 1, k), kidx == 0);
                    }
                }

                // sides
                for (int i = 0; i < N; ++i)
                {
                    AddTriangle(translateIndices(i, N, k), translateIndices(i + 1, N + 1, k),
                        translateIndices(i, N + 1, k), kidx == 0);
                    AddTriangle(translateIndices(i, N, k), translateIndices(i + 1, N, k),
                        translateIndices(i + 1, N + 1, k), kidx == 0);

                    AddTriangle(translateIndices(N, i, k), translateIndices(N + 1, i + 1, k),
                        translateIndices(N, i + 1, k), kidx == 0);
                    AddTriangle(translateIndices(N, i, k), translateIndices(N + 1, i, k),
                        translateIndices(N + 1, i + 1, k), kidx == 0);

                    AddTriangle(translateIndices(i + N + 1, N, k), translateIndices(i + N + 2, N + 1, k),
                        translateIndices(i + N + 1, N + 1, k), kidx == 0);
                    AddTriangle(translateIndices(i + N + 1, N, k), translateIndices(i + N + 2, N, k),
                        translateIndices(i + N + 2, N + 1, k), kidx == 0);

                    AddTriangle(translateIndices(N, i + N + 1, k), translateIndices(N + 1, i + N + 2, k),
                        translateIndices(N, i + N + 2, k), kidx == 0);
                    AddTriangle(translateIndices(N, i + N + 1, k), translateIndices(N + 1, i + N + 1, k),
                        translateIndices(N + 1, i + N + 2, k), kidx == 0);
                }

                // central
                AddTriangle(translateIndices(N, N, k), translateIndices(N + 1, N + 1, k), translateIndices(N, N + 1, k),
                    kidx == 0);
                AddTriangle(translateIndices(N, N, k), translateIndices(N + 1, N, k), translateIndices(N + 1, N + 1, k),
                    kidx == 0);
            }

            // xz-planes
            for (int kidx = 0; kidx < 2; ++kidx)
            {
                int k = ks[kidx];
                Vector3 origin = new Vector3(-b[0] - radius, (b[1] + radius) * sign[kidx], -b[2] - radius);

                for (int j = 0; j <= N; ++j)
                {
                    for (int i = 0; i <= N; ++i)
                    {
                        Vector3 pos = origin + new Vector3(dx * i, 0f, dx * j);
                        AddVertex(i, k, j, pos, new Vector3(-b[0], b[1] * sign[kidx], -b[2]));

                        pos = origin + new Vector3(dx * i + 2f * b[0] + radius, 0f, dx * j);
                        AddVertex(i + N + 1, k, j, pos, new Vector3(b[0], b[1] * sign[kidx], -b[2]));

                        pos = origin + new Vector3(dx * i + 2f * b[0] + radius, 0f, dx * j + 2f * b[2] + radius);
                        AddVertex(i + N + 1, k, j + N + 1, pos, new Vector3(b[0], b[1] * sign[kidx], b[2]));

                        pos = origin + new Vector3(dx * i, 0f, dx * j + 2f * b[2] + radius);
                        AddVertex(i, k, j + N + 1, pos, new Vector3(-b[0], b[1] * sign[kidx], b[2]));
                    }
                }

                // corners
                for (int j = 0; j < N; ++j)
                {
                    for (int i = 0; i < N; ++i)
                    {
                        AddTriangle(translateIndices(i, k, j), translateIndices(i + 1, k, j + 1),
                            translateIndices(i, k, j + 1), kidx == 1);
                        AddTriangle(translateIndices(i, k, j), translateIndices(i + 1, k, j),
                            translateIndices(i + 1, k, j + 1), kidx == 1);

                        AddTriangle(translateIndices(i, k, j + N + 1), translateIndices(i + 1, k, j + N + 2),
                            translateIndices(i, k, j + N + 2), kidx == 1);
                        AddTriangle(translateIndices(i, k, j + N + 1), translateIndices(i + 1, k, j + N + 1),
                            translateIndices(i + 1, k, j + N + 2), kidx == 1);

                        AddTriangle(translateIndices(i + N + 1, k, j + N + 1),
                            translateIndices(i + N + 2, k, j + N + 2), translateIndices(i + N + 1, k, j + N + 2),
                            kidx == 1);
                        AddTriangle(translateIndices(i + N + 1, k, j + N + 1),
                            translateIndices(i + N + 2, k, j + N + 1), translateIndices(i + N + 2, k, j + N + 2),
                            kidx == 1);


                        AddTriangle(translateIndices(i + N + 1, k, j), translateIndices(i + N + 2, k, j + 1),
                            translateIndices(i + N + 1, k, j + 1), kidx == 1);
                        AddTriangle(translateIndices(i + N + 1, k, j), translateIndices(i + N + 2, k, j),
                            translateIndices(i + N + 2, k, j + 1), kidx == 1);
                    }
                }

                // sides
                for (int i = 0; i < N; ++i)
                {
                    AddTriangle(translateIndices(i, k, N), translateIndices(i + 1, k, N + 1),
                        translateIndices(i, k, N + 1), kidx == 1);
                    AddTriangle(translateIndices(i, k, N), translateIndices(i + 1, k, N),
                        translateIndices(i + 1, k, N + 1), kidx == 1);

                    AddTriangle(translateIndices(N, k, i), translateIndices(N + 1, k, i + 1),
                        translateIndices(N, k, i + 1), kidx == 1);
                    AddTriangle(translateIndices(N, k, i), translateIndices(N + 1, k, i),
                        translateIndices(N + 1, k, i + 1), kidx == 1);

                    AddTriangle(translateIndices(i + N + 1, k, N), translateIndices(i + N + 2, k, N + 1),
                        translateIndices(i + N + 1, k, N + 1), kidx == 1);
                    AddTriangle(translateIndices(i + N + 1, k, N), translateIndices(i + N + 2, k, N),
                        translateIndices(i + N + 2, k, N + 1), kidx == 1);

                    AddTriangle(translateIndices(N, k, i + N + 1), translateIndices(N + 1, k, i + N + 2),
                        translateIndices(N, k, i + N + 2), kidx == 1);
                    AddTriangle(translateIndices(N, k, i + N + 1), translateIndices(N + 1, k, i + N + 1),
                        translateIndices(N + 1, k, i + N + 2), kidx == 1);
                }

                // central
                AddTriangle(translateIndices(N, k, N), translateIndices(N + 1, k, N + 1), translateIndices(N, k, N + 1),
                    kidx == 1);
                AddTriangle(translateIndices(N, k, N), translateIndices(N + 1, k, N), translateIndices(N + 1, k, N + 1),
                    kidx == 1);
            }

            // yz-planes
            for (int kidx = 0; kidx < 2; ++kidx)
            {
                int k = ks[kidx];
                Vector3 origin = new Vector3((b[0] + radius) * sign[kidx], -b[1] - radius, -b[2] - radius);
                for (int j = 0; j <= N; ++j)
                for (int i = 0; i <= N; ++i)
                {
                    Vector3 pos = origin + new Vector3(0f, dx * i, dx * j);
                    AddVertex(k, i, j, pos, new Vector3(b[0] * sign[kidx], -b[1], -b[2]));

                    pos = origin + new Vector3(0f, dx * i + 2f * b[1] + radius, dx * j);
                    AddVertex(k, i + N + 1, j, pos, new Vector3(b[0] * sign[kidx], b[1], -b[2]));

                    pos = origin + new Vector3(0f, dx * i + 2f * b[1] + radius, dx * j + 2f * b[2] + radius);
                    AddVertex(k, i + N + 1, j + N + 1, pos, new Vector3(b[0] * sign[kidx], b[1], b[2]));

                    pos = origin + new Vector3(0f, dx * i, dx * j + 2f * b[2] + radius);
                    AddVertex(k, i, j + N + 1, pos, new Vector3(b[0] * sign[kidx], -b[1], b[2]));
                }

                // corners
                for (int j = 0; j < N; ++j)
                {
                    for (int i = 0; i < N; ++i)
                    {
                        AddTriangle(translateIndices(k, i, j), translateIndices(k, i + 1, j + 1),
                            translateIndices(k, i, j + 1), kidx == 0);
                        AddTriangle(translateIndices(k, i, j), translateIndices(k, i + 1, j),
                            translateIndices(k, i + 1, j + 1), kidx == 0);

                        AddTriangle(translateIndices(k, i, j + N + 1), translateIndices(k, i + 1, j + N + 2),
                            translateIndices(k, i, j + N + 2), kidx == 0);
                        AddTriangle(translateIndices(k, i, j + N + 1), translateIndices(k, i + 1, j + N + 1),
                            translateIndices(k, i + 1, j + N + 2), kidx == 0);

                        AddTriangle(translateIndices(k, i + N + 1, j + N + 1),
                            translateIndices(k, i + N + 2, j + N + 2), translateIndices(k, i + N + 1, j + N + 2),
                            kidx == 0);
                        AddTriangle(translateIndices(k, i + N + 1, j + N + 1),
                            translateIndices(k, i + N + 2, j + N + 1), translateIndices(k, i + N + 2, j + N + 2),
                            kidx == 0);

                        AddTriangle(translateIndices(k, i + N + 1, j), translateIndices(k, i + N + 2, j + 1),
                            translateIndices(k, i + N + 1, j + 1), kidx == 0);
                        AddTriangle(translateIndices(k, i + N + 1, j), translateIndices(k, i + N + 2, j),
                            translateIndices(k, i + N + 2, j + 1), kidx == 0);
                    }
                }

                // sides
                for (int i = 0; i < N; ++i)
                {
                    AddTriangle(translateIndices(k, i, N), translateIndices(k, i + 1, N + 1),
                        translateIndices(k, i, N + 1), kidx == 0);
                    AddTriangle(translateIndices(k, i, N), translateIndices(k, i + 1, N),
                        translateIndices(k, i + 1, N + 1), kidx == 0);

                    AddTriangle(translateIndices(k, N, i), translateIndices(k, N + 1, i + 1),
                        translateIndices(k, N, i + 1), kidx == 0);
                    AddTriangle(translateIndices(k, N, i), translateIndices(k, N + 1, i),
                        translateIndices(k, N + 1, i + 1), kidx == 0);

                    AddTriangle(translateIndices(k, i + N + 1, N), translateIndices(k, i + N + 2, N + 1),
                        translateIndices(k, i + N + 1, N + 1), kidx == 0);
                    AddTriangle(translateIndices(k, i + N + 1, N), translateIndices(k, i + N + 2, N),
                        translateIndices(k, i + N + 2, N + 1), kidx == 0);

                    AddTriangle(translateIndices(k, N, i + N + 1), translateIndices(k, N + 1, i + N + 2),
                        translateIndices(k, N, i + N + 2), kidx == 0);
                    AddTriangle(translateIndices(k, N, i + N + 1), translateIndices(k, N + 1, i + N + 1),
                        translateIndices(k, N + 1, i + N + 2), kidx == 0);
                }

                // central
                AddTriangle(translateIndices(k, N, N), translateIndices(k, N + 1, N + 1), translateIndices(k, N, N + 1),
                    kidx == 0);
                AddTriangle(translateIndices(k, N, N), translateIndices(k, N + 1, N), translateIndices(k, N + 1, N + 1),
                    kidx == 0);
            }

        }

        void GenerateUnbeveled()
        {

            // Debug.Log("GenerateUnbeveled");
            Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
            Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);

            Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
            Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
            Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

            _vertices.AddRange(new Vector3[]
            {
                // Bottom
                p0, p1, p2, p3,

                // Left
                p7, p4, p0, p3,

                // Front
                p4, p5, p1, p0,

                // Back
                p6, p7, p3, p2,

                // Right
                p5, p6, p2, p1,

                // Top
                p7, p6, p5, p4
            });

            _normals.AddRange(new Vector3[]
            {
                Vector3.down, Vector3.down, Vector3.down, Vector3.down,
                Vector3.left, Vector3.left, Vector3.left, Vector3.left,
                Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,
                Vector3.back, Vector3.back, Vector3.back, Vector3.back,
                Vector3.right, Vector3.right, Vector3.right, Vector3.right,
                Vector3.up, Vector3.up, Vector3.up, Vector3.up
            });

            _triangles.AddRange(new int[]
            {
                // Bottom
                3, 1, 0,
                3, 2, 1,

                // Left
                3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

                // Front
                3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

                // Back
                3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

                // Right
                3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

                // Top
                3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            });

        }

        void Resize<T>(List<T> list, int sz, T c)
        {
            int cur = list.Count;
            if (sz < cur)
                list.RemoveRange(sz, cur - sz);
            else if (sz > cur)
            {
                if (sz > list.Capacity) //this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                    list.Capacity = sz;
                list.AddRange(Enumerable.Repeat(c, sz - cur));
            }
        }

        protected override void OnValidate()
        {
            _bevelSubdivisions = Mathf.Clamp(_bevelSubdivisions, 0, 20);
            bevel = Mathf.Max(bevel, 0f);

            base.OnValidate();
        }
    }


    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class ProceduralPrimitive : MonoBehaviour
    {
        //   [SerializeField]
        private bool _debugNormals;

        [SerializeField] private bool _autoRegenerate = true;

        [SerializeField] [HideInInspector] private Mesh _meshAsset;

        protected List<Vector3> _vertices = new List<Vector3>();
        protected List<Vector3> _normals = new List<Vector3>();
        protected List<Vector2> _uvs = new List<Vector2>();
        protected List<int> _triangles = new List<int>();
        private Vector3 _lastLocalScale;

        public void Generate()
        {
            _vertices.Clear();
            _normals.Clear();
            _uvs.Clear();
            _triangles.Clear();

            GenerateMesh();

            MeshFilter meshFilter = GetComponent<MeshFilter>();

            if (_meshAsset == null)
            {
                meshFilter.sharedMesh = new Mesh();
                meshFilter.sharedMesh.name = GetType().Name + " Mesh (SCENE ONLY)";
            }
            else
            {
                meshFilter.sharedMesh = _meshAsset;
            }

            meshFilter.sharedMesh.SetVertices(_vertices);
            meshFilter.sharedMesh.SetTriangles(_triangles, 0);
            meshFilter.sharedMesh.SetNormals(_normals);
            meshFilter.sharedMesh.SetUVs(0, _uvs);
            meshFilter.sharedMesh.RecalculateBounds();

#if UNITY_EDITOR
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null && meshRenderer.sharedMaterial == null)
            {
                meshRenderer.sharedMaterial = EditorUtils.GetDefaultMaterial();
                meshRenderer.sharedMaterial.color = Color.white;
            }
#endif

            _lastLocalScale = transform.localScale;
        }

        protected void AddTriangle(int indexA, int indexB, int indexC, bool invertWindingOrder = false)
        {
            if (invertWindingOrder)
            {
                _triangles.Add(indexC);
                _triangles.Add(indexB);
                _triangles.Add(indexA);
            }
            else
            {
                _triangles.Add(indexA);
                _triangles.Add(indexB);
                _triangles.Add(indexC);
            }
        }

        protected abstract void GenerateMesh();

        protected virtual void OnValidate()
        {
            if (_autoRegenerate)
            {
                Generate();
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (_autoRegenerate && transform.hasChanged)
            {
                transform.hasChanged = false;
                if (transform.localScale != _lastLocalScale)
                {
                    Generate();
                    _lastLocalScale = transform.localScale;
                }
            }
        }

        void OnDrawGizmos()
        {
            if (_debugNormals)
            {
                for (int i = 0; i < _vertices.Count; i++)
                {
                    DebugUtils.DrawLine(transform.TransformPoint(_vertices[i]),
                        transform.TransformPoint(_vertices[i] + _normals[i] * 0.2f),
                        new Color(_normals[i].x, _normals[i].y, _normals[i].z));
                }
            }
        }
    }
}