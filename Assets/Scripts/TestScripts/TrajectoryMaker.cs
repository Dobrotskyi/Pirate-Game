using UnityEngine;

public class TrajectoryMaker : MonoBehaviour
{
    private const int N = 30;
    private LineRenderer _trajectoryLine;
    Vector3[] points = new Vector3[N];

    public void SetTrajectoryLine(LineRenderer trajectoryLine)
    {
        _trajectoryLine = trajectoryLine;
        _trajectoryLine.positionCount = N;
    }

    public void ShowTrajectory(Vector3 origin, Vector3 velocity)
    {
        if(_trajectoryLine.enabled == false)
            _trajectoryLine.enabled = true;

        Vector3 lastCorrectPoint = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + (velocity / 2) * time + Physics.gravity * (time * time / 2f);

            if (points[i].y < 0)
            {
                if (points[i - 1] != Vector3.zero)
                    lastCorrectPoint = points[i - 1];
                points[i] = lastCorrectPoint;
            }
        }
        _trajectoryLine.SetPositions(points);
    }

    public void HideLines()
    {
        _trajectoryLine.enabled = false;
    }
}
