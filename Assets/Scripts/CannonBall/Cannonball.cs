using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Cannonball"))
            Destroy(gameObject);
    }
}
