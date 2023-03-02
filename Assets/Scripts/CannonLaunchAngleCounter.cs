using UnityEngine;

public static class CannonLaunchAngleCounter
{
    private const float G = 9.81f;

    public static float GetLaunchAngle(Transform origin, Transform target, Vector3 velocity)
    {
        float xDist = RelativeToMainDistanceX(origin, target);
        float yDist = target.position.y - origin.position.y;

        float mag = velocity.magnitude;
        float underSqr = Mathf.Pow(mag, 4) - G * (G * xDist * xDist + 2 * yDist * mag * mag);

        if (underSqr < 0)
            return float.NaN;

        float underATan = (mag * mag - Mathf.Sqrt(underSqr)) / (G * xDist);
        float launchAngle = Mathf.Atan(underATan) * Mathf.Rad2Deg;
        return launchAngle;
    }

    private static float RelativeToMainDistanceX(Transform origin, Transform target)
    {
        if (HaveSameMainParent(origin, target) == false)
            return -1;

        float distance = Vector3.Distance(origin.position, target.position);
        float xDistance = Mathf.Sqrt(Mathf.Pow(distance, 2) - Mathf.Pow(target.position.y - origin.position.y, 2));
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
