using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{

    float startTime;
    float endTime;


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Cannonball"))
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        startTime = Time.time;
    }

    private void OnDestroy()
    {
        endTime = Time.time;
    }
}
