using UnityEngine;
using System.Collections.Generic;


public class Villages : MonoBehaviour
{
    public Vector3 GetRandomDock
    {
        get
        {
            return _docks[Random.Range(0, _docks.Count)];
        }
    }

    private List<Vector3> _docks = new List<Vector3>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _docks.Add(transform.GetChild(i).position);
        }
    }
}
