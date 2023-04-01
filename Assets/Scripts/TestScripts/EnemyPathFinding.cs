using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPathFinding : MonoBehaviour
{
    internal bool _attackMode = false;
    internal Transform _target;
    private Transform _destinationTransform;
    private NavMeshAgent _navMeshAgent;
    private Villages _villages;
    private bool _destinationReached = true;

    private float _minimumAttackDistance = 20f;
    private float _startChasingDistance = 30f;

    [SerializeField]
    private Transform _leftCannonsPlacement;
    [SerializeField]
    private Transform _rightCannonsPlacement;
    private EnemyShipController _shipController;


    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _villages = FindObjectOfType<Villages>();

        _shipController = transform.GetComponentInChildren<EnemyShipController>();

        StartCoroutine(Docking());
    }

    private void FixedUpdate()
    {
        if (_attackMode == false && Vector3.Distance(transform.position, _navMeshAgent.destination) <= _navMeshAgent.stoppingDistance && _destinationReached == false)
        {
            _destinationReached = true;
            StartCoroutine(Docking());
        }

        if (_attackMode)
        {
            float distance = Vector3.Distance(transform.position, _target.position);
            if (distance < _minimumAttackDistance)
            {
                AimAtTarget();
            }
            else
            {
                _shipController.StopCannonsAiming();
                if (distance >= _startChasingDistance)
                {
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.destination = _target.position;
                }
            }
        }
    }

    private IEnumerator Docking()
    {
        yield return new WaitForSeconds(5);

        _navMeshAgent.destination = _villages.GetRandomDock;
        _destinationReached = false;
        yield break;
    }

    private void AimAtTarget()
    {
        _navMeshAgent.enabled = false;
        Vector3 direction = transform.position - _target.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        float rotateToSide;
        if (Vector3.Distance(_leftCannonsPlacement.position, _target.position) < Vector3.Distance(_rightCannonsPlacement.position, _target.position))
        {
            rotateToSide = -90;
            _shipController.AimLeftCannons();
            _shipController.ShootLeft();
        }
        else
        {
            rotateToSide = 90;
            _shipController.AimRightCannons();
            _shipController.ShootRight();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y + rotateToSide, 0), 4 * Time.deltaTime);
        _navMeshAgent.enabled = true;
    }
}
