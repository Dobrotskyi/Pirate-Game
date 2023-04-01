using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : Cannon
{
    protected override void BarrelAim()
    {
        Vector3 launchVector = _cannonShotForce * _cannonballSpawner.forward / _cannonballMass;
        Vector3 partToHit = _target.position;
        partToHit.y += _centerOfTargetYOffset;
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

    private bool CannonOnLeftSide()
    {
        if (transform.localPosition.x < 0)
            return true;
        else
            return false;
    }
}
