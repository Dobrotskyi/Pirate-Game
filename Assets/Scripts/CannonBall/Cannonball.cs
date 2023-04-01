using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffect;
    private float startLifeTime;
    private float endLifeTime;

    private void OnEnable()
    {
        startLifeTime = Time.time;
    }

    private void OnDestroy()
    {
        endLifeTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
        if (!collision.transform.CompareTag("Cannonball"))
            Destroy(gameObject);
    }
}
