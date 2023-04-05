using UnityEngine;

public static class CannonLaunchAngleCounter
{
    private const float G = 9.81f;

    public static float GetLaunchAngle(Vector3 origin, Vector3 target, Vector3 velocity)
    {
        float xDist = RelativeToMainDistanceX(origin, target);
        float yDist = target.y - origin.y;

        float mag = velocity.magnitude;
        float underSqr = Mathf.Pow(mag, 4) - G * (G * xDist * xDist + 2 * yDist * mag * mag);

        if (underSqr < 0)
            return float.NaN;

        float underATan = (mag * mag - Mathf.Sqrt(underSqr)) / (G * xDist);
        float launchAngle = Mathf.Atan(underATan) * Mathf.Rad2Deg;
        return launchAngle;
    }

    private static float RelativeToMainDistanceX(Vector3 origin, Vector3 target)
    {
        float distance = Vector3.Distance(origin, target);
        float xDistance = Mathf.Sqrt(Mathf.Pow(distance, 2) - Mathf.Pow(target.y - origin.y, 2));
        return xDistance;
    }

    private static bool HaveSameMainParent(Transform first, Transform second)
    {
        Transform firstMainParent = first;
        Transform secondMainParent = second;

        while (firstMainParent.parent != null)
            firstMainParent = firstMainParent.parent;

        while (secondMainParent.parent != null)
            secondMainParent = secondMainParent.parent;

        if (firstMainParent.gameObject != secondMainParent.gameObject)
        {
            Debug.Log("Error: Objects have different main parents");
            return false;
        }
        return true;
    }
}
