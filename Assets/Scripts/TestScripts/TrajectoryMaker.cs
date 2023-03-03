using System.Collections.Generic;
using UnityEngine;

public class TrajectoryMaker : MonoBehaviour
{
    private LineRenderer _trajectoryLine;

    public void SetTrajectoryLine(LineRenderer trajectoryLine)
    {
        _trajectoryLine = trajectoryLine;
    }

    public void ShowTrajectory(Vector3 origin, Vector3 velocity)
    {
        if(_trajectoryLine.enabled == false)
            _trajectoryLine.enabled = true;
        List<Vector3> points = new List<Vector3>();
        int i = 0;
        float time = i * 0.1f;
        points.Add(origin + velocity * time + Physics.gravity * (time * time / 2f));
        i++;
        
        while (points[points.Count - 1].y > 0) 
        {
            time = i * 0.1f;
            points.Add(origin + velocity * time + Physics.gravity * (time * time / 2f));
            i++;
        }

        _trajectoryLine.positionCount = points.Count;
        _trajectoryLine.SetPositions(points.ToArray());
    }

    public void HideTrajectory()
    {
        _trajectoryLine.enabled = false;
    }
}
