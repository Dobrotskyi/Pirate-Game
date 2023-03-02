using UnityEngine;

public static class CannonLaunchAngleCounter
{
    private const float G = 9.81f;

    public static float LaunchAngle(float xDist,float yDist, Vector3 velocity)
    {
        float mag = (velocity/2).magnitude;
        Debug.Log($"{xDist}, {yDist}");
        //float g = Physics.gravity.magnitude;

        float underSqr = Mathf.Pow(mag, 4) - G * (G * xDist * xDist + 2 * yDist * mag * mag);
        Debug.Log(underSqr);

        if (underSqr < 0)
            return float.NaN;

        float underATan = (mag * mag + Mathf.Sqrt(underSqr)) / (G * xDist);
        float launchAngle = Mathf.Atan(underATan) * Mathf.Rad2Deg;
        return launchAngle;
    }

    public static float GetLaunchAngle(Transform origin, Transform target, Vector3 velocity) 
    {
        float originX = GetXPosRelativeToMainGameObject(origin);
        float originY = GetYPosRelativeToMainGameObject(origin);

        float targetX = GetXPosRelativeToMainGameObject(target);
        float targetY = GetYPosRelativeToMainGameObject(target);

        float mag = (velocity/2).magnitude;//2 - Масса ядра

        return 0;
    }   
    
    private static float GetXPosRelativeToMainGameObject(Transform transform) 
    {


        return 0;
    }

    private static float GetYPosRelativeToMainGameObject(Transform transform) 
    {
        return 0;
    }
}
