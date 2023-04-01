using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : Cannon
{
    Rigidbody _targetRb;
    [SerializeField]
    private GameObject _aimingPoint;

    protected override void BarrelAim()
    {
        Vector3 launchVector = _cannonShotForce * _cannonballSpawner.forward / _cannonballMass;
        Vector3 partToHit = GetAimingPoint();
        partToHit.y += _centerOfTargetYOffset;
        _aimingPoint.transform.position = partToHit;
        float launchAngle = CannonLaunchAngleCounter.GetLaunchAngle(_cannonballSpawner.position, partToHit, launchVector);
        if (float.IsNaN(launchAngle))
            Debug.Log("Target out of reach");
        else
        {
            float shipZRotation = _shipRb.rotation.eulerAngles.z;
            float includingShipRotationAngle;

            if (CannonOnLeftSide())
            {
                shipZRotation = 360 - shipZRotation;
            }
            else
            {
                shipZRotation = Mathf.Abs(0 - shipZRotation);
            }

            includingShipRotationAngle = launchAngle - shipZRotation;
            _barrel.transform.localRotation = Quaternion.Euler(includingShipRotationAngle, 0, 0);
        }

        _trajectoryMaker.ShowTrajectory(_cannonballSpawner.position, launchVector);
    }

    protected override Vector3 GetAimingPoint()
    {
        if (_targetRb == null)
            _targetRb = _target.GetComponent<Rigidbody>();
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), _target.forward);
        Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), _targetRb.velocity);
        horizontalVelocity = forward * horizontalVelocity.magnitude;
        return _target.position + horizontalVelocity * GetCannonballTimeOfFlight();
    }

    private float GetCannonballTimeOfFlight()
    {
        return (Vector3.Distance(_cannonballSpawner.position, _target.position)) / (GetLaunchVector().magnitude * Mathf.Cos(transform.localRotation.eulerAngles.x));
    }

    private bool CannonOnLeftSide()
    {
        if (transform.localPosition.x < 0)
            return true;
        else
            return false;
    }
}
