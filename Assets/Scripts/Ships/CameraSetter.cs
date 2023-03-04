using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    [SerializeField] private GameObject _createdParentCMCameras;

    private void OnEnable()
    {
        List<GameObject> cameras = new List<GameObject>();
        for (int i = 0; i < _createdParentCMCameras.transform.childCount; i++)
            cameras.Add(_createdParentCMCameras.transform.GetChild(i).gameObject);

        GameObject.Find("MainCamera").GetComponent<ShipCameraController>().SetCameras(cameras);
            _createdParentCMCameras.transform.parent = null;
    }

    private void OnDisable()
    {
        Destroy(_createdParentCMCameras);
    }
}
