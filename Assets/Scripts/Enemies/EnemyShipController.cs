using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : ShipController
{
    private Transform _target;

    protected override void OnEnable()
    {
        _leftCannons = new List<Cannon>();
        _rightCannons = new List<Cannon>();

        SetCannons();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _target = other.transform;
            SetNewTargetToCannons();
        }
    }

    private void SetNewTargetToCannons()
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.SetTarget(_target);
    }
}
