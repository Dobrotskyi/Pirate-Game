using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLinesCreator : MonoBehaviour
{
    [SerializeField] private GameObject _trajectoryLine;

    private void OnEnable()
    {
        GameObject cannons = transform.parent.Find("Cannons").gameObject;

        for (int i = 0; i < cannons.transform.childCount; i++)
        {
            GameObject line = Instantiate(_trajectoryLine);
            line.transform.parent = transform;
            line.transform.localPosition = transform.localPosition;
            line.gameObject.SetActive(false);
            
            cannons.transform.GetChild(i).GetComponent<TrajectoryMaker>().SetTrajectoryLine(line.GetComponent<LineRenderer>());
        }
    }
}
