using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : ShipController
{
    private const float VELOCITY_MULTIPLIER = 100f;

    [SerializeField] private float _allowedDistanceToPathFinder = 5.5f;
    [SerializeField] private float _distanceWhenPathFinderToFar = 6f;
    [SerializeField] private Transform _leftCannonsPlacement;
    [SerializeField] private Transform _rightCannonsPlacement;
    [SerializeField] private float _chaseDistance = 5f;
    [SerializeField] private float _shootingDistance = 10f;

    private Vector2 _speedAndMaxSpeed = new Vector2(600, 900);
    private EnemyPathFinder _pathFinder;
    private Rigidbody _rb;
    private Transform _target;
    private bool canGoForward = true;

    internal void Attack()
    {
        if (_pathFinder.NavMeshAgent.isStopped == false)
            _pathFinder.NavMeshAgent.isStopped = true;

        float rotateToSide;
        if (Vector3.Distance(_leftCannonsPlacement.position, _target.position) < Vector3.Distance(_rightCannonsPlacement.position, _target.position))
        {
            AimCannons(_leftCannons);
            rotateToSide = -90;
        }
        else
        {
            AimCannons(_rightCannons);
            rotateToSide = 90;
        }
        Vector3 direction = transform.position - _target.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y + rotateToSide, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), 4 * Time.deltaTime);
        if (canGoForward)
            MoveForward(_speedAndMaxSpeed.x);

        Vector3 newPos = transform.localPosition;
        newPos.y = 0;
        _pathFinder.transform.localPosition = newPos;
    }

    internal void FollowPathFinder()
    {
        if (_pathFinder.NavMeshAgent.isStopped == true)
            _pathFinder.NavMeshAgent.isStopped = false;

        float distanceToPathFinder = Vector3.Distance(_pathFinder.transform.position, transform.position);
        if (distanceToPathFinder > _allowedDistanceToPathFinder)
        {
            Vector3 direction = transform.position - _pathFinder.transform.position;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            Quaternion lerpRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), _pathFinder.NavMeshAgent.angularSpeed * Time.deltaTime);
            lerpRotation.x = transform.rotation.x;
            lerpRotation.z = transform.rotation.z;
            transform.rotation = lerpRotation;
        }

        if (distanceToPathFinder > _distanceWhenPathFinderToFar)
            MoveForward(_pathFinder.TrackVelocity * 1.2f * VELOCITY_MULTIPLIER);
        else
        if (distanceToPathFinder > _allowedDistanceToPathFinder)
            MoveForward(_pathFinder.TrackVelocity * VELOCITY_MULTIPLIER);
    }

    internal void SetNewTarget(Transform target)
    {
        _target = target;
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        foreach (Cannon cannon in _leftCannons)
            cannon.SetTarget(target);
        foreach (Cannon cannon in _rightCannons)
            cannon.SetTarget(target);
    }

    internal bool TargetToFarToShoot()
    {
        if (Vector3.Distance(_target.position, transform.position) > _shootingDistance + _pathFinder.NavMeshAgent.stoppingDistance)
            return true;
        else
            return false;
    }

    internal bool ReadyToAttack()
    {
        if (Vector3.Distance(_target.position, transform.position) < _chaseDistance + _pathFinder.NavMeshAgent.stoppingDistance)
            return true;
        else
            return false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _rb = GetComponent<Rigidbody>();
        _pathFinder = transform.parent.Find("PathFinder").GetComponent<EnemyPathFinder>();
        _speedAndMaxSpeed = new Vector2(_pathFinder.NavMeshAgent.acceleration, _pathFinder.NavMeshAgent.speed) * VELOCITY_MULTIPLIER;

    }

    private void MoveForward(float velocity)
    {
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        _rb.AddForce(forward * velocity * Time.deltaTime, ForceMode.Acceleration);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _speedAndMaxSpeed.y);
    }

    private void AimCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

    private void FixedUpdate()
    {
        KeepHorizontalVelocityForward();
    }

    private void KeepHorizontalVelocityForward()
    {
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), _rb.velocity);
        horizontalVelocity = forward * horizontalVelocity.magnitude;
        float verticalVelocity = _rb.velocity.y;
        _rb.velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
            canGoForward = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Terrain"))
            canGoForward = true;
    }
}
