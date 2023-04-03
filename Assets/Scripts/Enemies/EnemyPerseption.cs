using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerseption : MonoBehaviour
{
    internal enum BehaviourStates
    {
        goingToDock,
        docked,
        attacking,
        chasingTarget
    }

    internal BehaviourStates _currentBehaviourState;

    [SerializeField] private EnemyShipController _shipController;
    [SerializeField] private EnemyPathFinder _pathFinder;

    private Transform _currentTarget;

    private void OnEnable()
    {
        //StartCoroutine(_pathFinder.Docking());
        _currentBehaviourState = BehaviourStates.docked;
    }

    private void FixedUpdate()
    {
        transform.position = _shipController.transform.position;
        if (_currentBehaviourState == BehaviourStates.docked && _pathFinder.SearchingForNewDock == false)
        {
            _pathFinder.FindNewDock();
            _currentBehaviourState = BehaviourStates.goingToDock;
        }
        else
        {
            if (_currentBehaviourState == BehaviourStates.goingToDock)
            {
                _shipController.FollowPathFinder();
                if(_pathFinder.ShipHasReachedDock() && _pathFinder.SearchingForNewDock == false)
                    _currentBehaviourState = BehaviourStates.docked;
            }
        }

        if (_currentTarget != null && _currentBehaviourState == BehaviourStates.chasingTarget)
        {
            _pathFinder.SetNewDestination(_currentTarget.position);
            _shipController.FollowPathFinder();
            if (_shipController.ReadyToAttack())
            {
                _currentBehaviourState = BehaviourStates.attacking;
            }
        }

        if (_currentBehaviourState == BehaviourStates.attacking)
        {
            if (_shipController.TargetToFarToShoot())
            {
                _currentBehaviourState = BehaviourStates.chasingTarget;
                _shipController.StopCannonsAiming();
            }
            else
            {
                _shipController.Attack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _currentTarget = other.transform;
            _shipController.SetNewTarget(_currentTarget);
            _currentBehaviourState = BehaviourStates.chasingTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _currentTarget = null;
            _currentBehaviourState = BehaviourStates.goingToDock;
        }
    }
}
