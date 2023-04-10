using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : ShipController
{
    public static event Action Destroyed;
    public event Action Emergency;

    private const float VELOCITY_MULTIPLIER = 100f;

    [SerializeField] private float _allowedDistanceToPathFinder = 5.5f;
    [SerializeField] private float _fastFollowPathfinderDistance = 6f;
    [SerializeField] private float _pathFinderTooFarToChase = 15f;
    [SerializeField] private Transform _leftCannonsPlacement;
    [SerializeField] private Transform _rightCannonsPlacement;
    [SerializeField] private float _chaseDistance = 5f;
    [SerializeField] private float _shootingDistance = 10f;

    private Vector2 _speedAndMaxSpeed = new Vector2(600, 900);
    private EnemyPathFinder _pathFinder;
    private Transform _target;
    private bool _canGoForward = true;
    private float _rotationSpeed = 1f;

    internal void Attack()
    {
        if (_pathFinder.navMeshAgent.isStopped == false)
            _pathFinder.navMeshAgent.isStopped = true;

        AimAndShoot();
        MoveForward(_speedAndMaxSpeed.x);
        KeepPathfinderClose();
    }

    internal void FollowPathFinder()
    {
        if (_pathFinder.navMeshAgent.isStopped == true)
            _pathFinder.navMeshAgent.isStopped = false;

        float distanceToPathFinder = Vector3.Distance(_pathFinder.transform.position, transform.position);

        if (distanceToPathFinder > _allowedDistanceToPathFinder)
        {
            RotateYToTarget(_pathFinder.transform.position);
        }

        if (distanceToPathFinder > _pathFinderTooFarToChase)
            KeepPathfinderClose();
        else
        if (distanceToPathFinder > _fastFollowPathfinderDistance)
            MoveForward(_pathFinder.TrackVelocity * 1.35f * VELOCITY_MULTIPLIER);

        else
        if (distanceToPathFinder > _allowedDistanceToPathFinder)
            MoveForward(_pathFinder.TrackVelocity * VELOCITY_MULTIPLIER);
    }

    private void AimAndShoot()
    {
        float rotateToSide;
        if (Vector3.Distance(_leftCannonsPlacement.position, _target.position) < Vector3.Distance(_rightCannonsPlacement.position, _target.position))
        {
            AimCannons(_leftCannons);
            if (CannonsAreAimedAtTarget(_leftCannons))
                ShootLeft();
            rotateToSide = 90;
        }
        else
        {
            AimCannons(_rightCannons);
            if (CannonsAreAimedAtTarget(_rightCannons))
                ShootRight();
            rotateToSide = -90;
        }
        RotateYToTarget(_target.position, rotateToSide);

        if (_shipCharacteristics.NoCannonballs && _target != null)
        {
            Debug.Log("Emergency invoke");
            _target = null;
            Emergency?.Invoke();
        }
    }

    private void KeepPathfinderClose()
    {
        Vector3 newPos = transform.localPosition;
        newPos.y = 0;
        _pathFinder.transform.localPosition = newPos;
    }

    private bool CannonsAreAimedAtTarget(List<Cannon> cannons)
    {
        foreach (EnemyCannon cannon in cannons)
        {
            if (cannon.AimedAtTarget() == false)
                return false;
        }
        return true;
    }

    private void RotateYToTarget(Vector3 target, float offset = 0)
    {
        Vector3 direction = transform.position - target;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        Quaternion lerpRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y + offset, 0), _rotationSpeed * Time.deltaTime);
        lerpRotation.x = transform.rotation.x;
        lerpRotation.z = transform.rotation.z;
        transform.rotation = lerpRotation;
    }

    private void MoveForward(float velocity)
    {
        if (_canGoForward)
        {
            Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
            _rb.AddForce(forward * velocity * Time.deltaTime, ForceMode.Acceleration);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _speedAndMaxSpeed.y);
        }
    }

    internal void SetNewTarget(Transform target)
    {
        _target = target;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        foreach (Cannon cannon in _leftCannons)
            cannon.SetTarget(targetRb);
        foreach (Cannon cannon in _rightCannons)
            cannon.SetTarget(targetRb);
    }

    internal bool TargetToFarToShoot()
    {
        if (Vector3.Distance(_target.position, transform.position) > _shootingDistance + _pathFinder.navMeshAgent.stoppingDistance)
            return true;
        else
            return false;
    }

    internal bool ReadyToAttack()
    {
        if (Vector3.Distance(_target.position, transform.position) < _chaseDistance + _pathFinder.navMeshAgent.stoppingDistance)
            return true;
        else
            return false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _rb = GetComponent<Rigidbody>();
        _pathFinder = transform.parent.Find("PathFinder").GetComponent<EnemyPathFinder>();
        _speedAndMaxSpeed = new Vector2(_pathFinder.navMeshAgent.acceleration, _pathFinder.navMeshAgent.speed) * VELOCITY_MULTIPLIER;
        _rotationSpeed = _pathFinder.navMeshAgent.angularSpeed / 60;
    }

    private void AimCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

    private void OnDestroy()
    {
        Transform mainObj = transform;
        while (mainObj.parent != null)
            mainObj = transform.parent;

        Destroy(mainObj.gameObject);
        Destroyed?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged") == false)
            _canGoForward = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged") == false)
            _canGoForward = true;

    }
}
