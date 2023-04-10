using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLookAtPlayer : MonoBehaviour
{
    private Transform _player;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 direction = transform.position - _player.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Euler(0,rotation.eulerAngles.y, 0);
    }
}
