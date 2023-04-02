using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : ShipController
{

    public Rigidbody Rb
    {
        get;
        private set;
    }

    public void AimRightCannons()
    {
        AimCannons(_rightCannons);
    }

    public void AimLeftCannons()
    {
        AimCannons(_leftCannons);
    }
    public void MoveForward(float velocity)
    {
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        Rb.AddForce(forward * velocity * 100 * Time.deltaTime, ForceMode.Acceleration);
        Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, 1000);
    }

    public void FollowPathFinder(EnemyPathFinder pathFinder)
    {
        Vector3 direction = transform.position - pathFinder.transform.position;
        Quaternion rotation = Quaternion.LookRotation(-direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), pathFinder.NavMeshAgent.angularSpeed * Time.deltaTime);
        if (Vector3.Distance(pathFinder.transform.position, transform.position) > 0.2f)
            MoveForward(pathFinder.TrackVelocity);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Rb = GetComponent<Rigidbody>();
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
        Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), Rb.velocity);
        horizontalVelocity = forward * horizontalVelocity.magnitude;
        float verticalVelocity = Rb.velocity.y;
        Rb.velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
    }
}
