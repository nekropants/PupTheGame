using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public abstract class ProceduralPrimitive : MonoBehaviour
{
    //   [SerializeField]
    private bool _debugNormals;

    [SerializeField]
    private bool _autoRegenerate = true;

    [SerializeField]
    [HideInInspector]
    private Mesh _meshAsset;

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
                DebugUtils.DrawLine(transform.TransformPoint(_vertices[i]), transform.TransformPoint(_vertices[i] + _normals[i] * 0.2f), new Color(_normals[i].x, _normals[i].y, _normals[i].z));
            }
        }
    }

}