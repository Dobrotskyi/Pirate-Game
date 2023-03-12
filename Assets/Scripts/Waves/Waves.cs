using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] private int _dimensions = 10;
    [SerializeField] private float UVScale;
    [SerializeField] private Octave[] _octaves;

    protected MeshFilter _meshFilter;
    protected Mesh _mesh;

    [Serializable]
    public struct Octave
    {
        public Vector2 Speed;
        public Vector2 Scale;
        public float Height;
        public bool Alternate;
    }

    public float GetHeight(Vector3 position)
    {
        //scale factor and position in local space
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, _dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, _dimensions);
        p2.x = Mathf.Clamp(p2.x, 0, _dimensions);
        p2.z = Mathf.Clamp(p2.z, 0, _dimensions);
        p3.x = Mathf.Clamp(p3.x, 0, _dimensions);
        p3.z = Mathf.Clamp(p3.z, 0, _dimensions);
        p4.x = Mathf.Clamp(p4.x, 0, _dimensions);
        p4.z = Mathf.Clamp(p4.z, 0, _dimensions);

        //get the max distance to one of the edges and take that to compute max - dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                 + (max - Vector3.Distance(p2, localPos))
                 + (max - Vector3.Distance(p3, localPos))
                 + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        //weighted sum
        var height = _mesh.vertices[MatrixIndex((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
                   + _mesh.vertices[MatrixIndex((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
                   + _mesh.vertices[MatrixIndex((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
                   + _mesh.vertices[MatrixIndex((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

        //scale
        return height * transform.lossyScale.y / dist;
    }

    private void Start()
    {
        _mesh = new Mesh();
        _mesh.name = gameObject.name;
        _mesh.vertices = GenerateVerts();
        _mesh.triangles = GenerateTriangles();
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        _mesh.uv = ReacalculateUVs();

        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;

    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(_dimensions + 1) * (_dimensions + 1)];
        for (int x = 0; x <= _dimensions; x++)
            for (int z = 0; z <= _dimensions; z++)
                verts[MatrixIndex(x, z)] = new Vector3(x, 0, z);

        return verts;
    }

    private int[] GenerateTriangles()
    {
        var triangles = new int[_mesh.vertices.Length * 6];

        for (int x = 0; x < _dimensions; x++)
            for (int z = 0; z < _dimensions; z++)
            {
                triangles[MatrixIndex(x, z) * 6 + 0] = MatrixIndex(x, z);
                triangles[MatrixIndex(x, z) * 6 + 1] = MatrixIndex(x + 1, z + 1);
                triangles[MatrixIndex(x, z) * 6 + 2] = MatrixIndex(x + 1, z);
                triangles[MatrixIndex(x, z) * 6 + 3] = MatrixIndex(x, z);
                triangles[MatrixIndex(x, z) * 6 + 4] = MatrixIndex(x, z + 1);
                triangles[MatrixIndex(x, z) * 6 + 5] = MatrixIndex(x + 1, z + 1);
            }
        return triangles;
    }

    private Vector2[] ReacalculateUVs()
    {
        var uvs = new Vector2[_mesh.vertices.Length];

        for (int x = 0; x <= _dimensions; x++)
            for (int z = 0; z <= _dimensions; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[MatrixIndex(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }

        return uvs;
    }

    private int MatrixIndex(int x, int z)
    {
        return x * (_dimensions + 1) + z;
    }

    private void FixedUpdate()//Should try fixedUpdate
    {
        var verts = _mesh.vertices;
        for (int x = 0; x <= _dimensions; x++)
            for (int z = 0; z <= _dimensions; z++)
            {
                float y = 0f;
                foreach (var octave in _octaves)
                {
                    if (octave.Alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octave.Scale.x) / _dimensions, (z * octave.Scale.y) / _dimensions) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + octave.Speed.magnitude * Time.time) * octave.Height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octave.Scale.x + Time.time * octave.Speed.x) / _dimensions, (z * octave.Scale.y + Time.time * octave.Speed.y) / _dimensions) - 0.5f;
                        y += perl * octave.Height;
                    }
                }
                verts[MatrixIndex(x, z)] = new Vector3(x, y, z);
            }
        _mesh.vertices = verts;
        _mesh.RecalculateNormals();
    }
}
