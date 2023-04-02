using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : ShipController
{
    [SerializeField] private float _allowedDistanceToPathFinder = 5.5f;
    [SerializeField] private float _distanceWhenPathFinderToFar = 6f;
    private Rigidbody _rb;
    private const float velocityMultiplier = 100f;


    public void AimRightCannons()
    {
        AimCannons(_rightCannons);
    }

    public void AimLeftCannons()
    {
        AimCannons(_leftCannons);
    }


    public void FollowPathFinder(EnemyPathFinder pathFinder)
    {
        Vector3 direction = transform.position - pathFinder.transform.position;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        Quaternion lerpRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), pathFinder.NavMeshAgent.angularSpeed * Time.deltaTime);
        lerpRotation.x = transform.rotation.x;
        lerpRotation.z = transform.rotation.z;
        transform.rotation = lerpRotation;

        float distanceToPathFinder = Vector3.Distance(pathFinder.transform.position, transform.position);
        if (distanceToPathFinder > _distanceWhenPathFinderToFar)
            MoveForward(pathFinder.TrackVelocity * 1.2f, pathFinder.NavMeshAgent.speed);
        else
        if (distanceToPathFinder > _allowedDistanceToPathFinder)
            MoveForward(pathFinder.TrackVelocity, pathFinder.NavMeshAgent.speed);

    }
    private void MoveForward(float velocity, float maxSpeed)
    {
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        _rb.AddForce(forward * velocity * velocityMultiplier * Time.deltaTime, ForceMode.Acceleration);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed * velocityMultiplier);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _rb = GetComponent<Rigidbody>();
    }

    private void AimCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

    internal void SetNewTargetToCannons(Rigidbody target)
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.SetTarget(target);
        foreach (Cannon cannon in _rightCannons)
            cannon.SetTarget(target);
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
}
