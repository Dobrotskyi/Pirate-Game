using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : ShipController
{
    private EnemyPathFinding _enemyPathFinding;

    public void AimRightCannons()
    {
        AimCannons(_rightCannons);
    }

    public void AimLeftCannons()
    {
        AimCannons(_leftCannons);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _enemyPathFinding = transform.GetComponentInParent<EnemyPathFinding>();
    }

    private void AimCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            SetNewTargetToCannons(other.transform.GetComponent<Rigidbody>());
            _enemyPathFinding._attackMode = true;
            _enemyPathFinding._target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _enemyPathFinding._attackMode = false;
        }
    }

    private void SetNewTargetToCannons(Rigidbody target)
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.SetTarget(target);
        foreach (Cannon cannon in _rightCannons)
            cannon.SetTarget(target);
    }
}
