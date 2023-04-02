using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerseption : MonoBehaviour
{
    private EnemyShipController _shipController;
    private EnemyPathFinder _pathFinder;

    private enum BehaviourStates
    {
        goingToDock,
        docked,
        attacking,
        chasingTarget
    }

    private BehaviourStates _currentBehaviourState;

    private void OnEnable()
    {
        _shipController = GetComponentInChildren<EnemyShipController>();
        _pathFinder = GetComponentInChildren<EnemyPathFinder>();

        StartCoroutine(_pathFinder.Docking());
        _currentBehaviourState = BehaviourStates.goingToDock;
    }

    private void FixedUpdate()
    {
        if (ShipHasReachedDock() && _currentBehaviourState == BehaviourStates.goingToDock)
        {
            _currentBehaviourState = BehaviourStates.docked;
            StartCoroutine(_pathFinder.Docking());
        }
        else
        {
            if (!ShipHasReachedDock())
            {
                _currentBehaviourState = BehaviourStates.goingToDock;
                _shipController.FollowPathFinder(_pathFinder);
            }
            }
    }

    private bool ShipHasReachedDock()
    {
        if (Vector3.Distance(transform.position, _pathFinder.NavMeshAgent.destination) <= _pathFinder.NavMeshAgent.stoppingDistance)
            return true;
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _shipController.SetNewTargetToCannons(other.transform.GetComponent<Rigidbody>());
            _pathFinder._target = other.transform;
            _currentBehaviourState = BehaviourStates.attacking;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _currentBehaviourState = BehaviourStates.goingToDock;
        }
    }


}
