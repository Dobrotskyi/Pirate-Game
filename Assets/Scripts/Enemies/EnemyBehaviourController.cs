using UnityEngine;

public class EnemyBehaviourController : MonoBehaviour
{
    [SerializeField] private EnemyShipController _shipController;
    [SerializeField] private EnemyPathFinder _pathFinder;
    private enum BehaviourStates
    {
        goingToDock,
        docked,
        attacking,
        chasingTarget
    }
    private BehaviourStates _currentBehaviourState;
    private Transform _currentTarget;
    private bool _ignoreTarget = false;

    private void OnEnable()
    {
        _currentBehaviourState = BehaviourStates.docked;
        _shipController.Emergency += GoDockIgnoreTarget;
    }

    private void OnDisable()
    {
        _shipController.Emergency -= GoDockIgnoreTarget;
    }

    private void GoDockIgnoreTarget()
    {
        _shipController.StopCannonsAiming();
        _currentBehaviourState = BehaviourStates.docked;
        _ignoreTarget = true;
    }

    private void FixedUpdate()
    {
        if (Dock.PlayerInDock || _shipController == null)
            return;

        transform.position = _shipController.transform.position;
        if (_currentBehaviourState == BehaviourStates.docked)
        {
            _pathFinder.FindNewDock();
            _currentBehaviourState = BehaviourStates.goingToDock;
        }
        else
        {
            if (_currentBehaviourState == BehaviourStates.goingToDock)
            {
                _shipController.FollowPathFinder();
                if (_pathFinder.ShipHasReachedDock() && _pathFinder.SearchingForNewDock == false)
                {
                    _currentBehaviourState = BehaviourStates.docked;
                    _ignoreTarget = false;
                    _shipController.Restock();
                }
            }
        }

        if (_ignoreTarget == false && _currentTarget != null)
        {
            if (_currentBehaviourState == BehaviourStates.chasingTarget)
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
        else if (_currentBehaviourState == BehaviourStates.attacking || _currentBehaviourState == BehaviourStates.chasingTarget
                && _currentTarget == null)
        {
            _shipController.StopCannonsAiming();
            _currentBehaviourState = BehaviourStates.docked;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip") && _ignoreTarget == false)
        {
            _currentTarget = other.transform;
            _shipController.SetNewTarget(_currentTarget);
            _currentBehaviourState = BehaviourStates.chasingTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip") && _ignoreTarget == false)
        {
            _shipController.StopCannonsAiming();
            _currentTarget = null;
            _currentBehaviourState = BehaviourStates.docked;
        }
    }
}
