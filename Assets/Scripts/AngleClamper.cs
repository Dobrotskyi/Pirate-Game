using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class AngleClamper
{
    public static float ClampAngle(float angle, float min, float max)
    {
        bool left = false;
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        float newAngle = angle;

        if (min > max)
            if (angle > max + Mathf.Abs(max - min) / 2)
                left = true;

        if (max > min)
            if (angle < min + Mathf.Abs(max - min) / 2)
                left = true;

        if (left && angle < min)
            newAngle = min;
        else if (!left && angle > max)
            newAngle = max;

        if (min == 0)
        {
            if (Mathf.Abs(max - angle) > Mathf.Abs(360 - angle))
                newAngle = min;
        }

        return newAngle;
    }
}
