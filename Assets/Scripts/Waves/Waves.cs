using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] private int _dimensions = 10;
    [SerializeField] private float UVScale;

    [Serializable]
    public struct GerstenerWave
    {
        public readonly Vector2 Direction;
        public readonly float Steepness;
        public readonly float Length;
        public readonly float Speed;

        public GerstenerWave(Vector2 direction, float steepness, float length, float speed)
        {
            Direction = direction;
            Steepness = steepness;
            Length = length;
            Speed = speed;
        }
    }

    private GerstenerWave[] _waves;
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    public float GetHeight(Vector3 position)
    {
        float time = Time.timeSinceLevelLoad;
        Vector3 currentPosition = GetWaveAddition(position, time);

        for (int i = 0; i < 3; i++)
        {
            Vector3 diff = new Vector3(position.x - currentPosition.x, 0, position.z - currentPosition.z);
            currentPosition = GetWaveAddition(diff, time);
        }

        return currentPosition.y;
    }

    private Vector3 GetWaveAddition(Vector3 position, float time)
    {
        Vector3 result = Vector3.zero;

        foreach (GerstenerWave wave in _waves)
        {
            float k = 2 * Mathf.PI / wave.Length;

            Vector2 normalizedDirection = wave.Direction.normalized;

            float f = k * (Vector2.Dot(normalizedDirection, new Vector2(position.x, position.z)) - (wave.Speed * time));
            float a = wave.Steepness / k;

            result += new Vector3(normalizedDirection.x * a * Mathf.Cos(f), a * Mathf.Sin(f), normalizedDirection.y * a * Mathf.Cos(f));
        }
        return result;
    }

    private void Awake()
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
        _mesh.vertices = _meshFilter.mesh.vertices;

        _waves = GetWaveDataFromMaterial();
    }

    private GerstenerWave[] GetWaveDataFromMaterial()
    {
        Material material = GetComponent<Renderer>().material;
        if (material != null)
        {
            GerstenerWave[] waves = {
                new GerstenerWave(new Vector2(material.GetVector("_WaveA").x,material.GetVector("_WaveA").y ),material.GetVector("_WaveA").z,material.GetVector("_WaveA").w, material.GetFloat("_Speed1")),
                new GerstenerWave(new Vector2(material.GetVector("_WaveB").x,material.GetVector("_WaveB").y ),material.GetVector("_WaveB").z,material.GetVector("_WaveB").w, material.GetFloat("_Speed2")),
                new GerstenerWave(new Vector2(material.GetVector("_WaveC").x,material.GetVector("_WaveC").y ),material.GetVector("_WaveC").z,material.GetVector("_WaveC").w, material.GetFloat("_Speed3"))
            };
            return waves;
        }
        else return null;
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
}
