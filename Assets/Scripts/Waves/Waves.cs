using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
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
        Vector3 result = new Vector3();

        foreach (GerstenerWave wave in _waves)
        {
            float k = 2 * Mathf.PI / wave.Length;

            Vector2 normalizedDirection = wave.Direction.normalized;

            float f = k * (Vector2.Dot(normalizedDirection, new Vector2(position.x, position.z)) - (wave.Speed * time));
            float a = wave.Steepness / k;

            result += new Vector3(normalizedDirection.x * (a * Mathf.Cos(f)), a * Mathf.Sin(f), normalizedDirection.y * (a * Mathf.Cos(f)));
        }
        return result;
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

    private void Awake()
    {
        _waves = GetWaveDataFromMaterial();
    }
}
