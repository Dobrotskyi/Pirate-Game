using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffect;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
        if (!collision.transform.CompareTag("Cannonball"))
            Destroy(gameObject);
    }
}
