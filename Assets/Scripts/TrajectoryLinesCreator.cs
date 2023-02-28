using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLinesCreator : MonoBehaviour
{
    [SerializeField] private GameObject _trajectoryDrawer;

    private void OnEnable()
    {
        GameObject cannons = transform.parent.Find("Cannons").gameObject;

        for (int i = 0; i < cannons.transform.childCount; i++)
        {
            GameObject line = Instantiate(_trajectoryDrawer, transform);
            cannons.transform.GetChild(i).GetComponent<TrajectoryMaker>().SetTrajectoryLine(line.GetComponent<LineRenderer>());
        }
    }
}
