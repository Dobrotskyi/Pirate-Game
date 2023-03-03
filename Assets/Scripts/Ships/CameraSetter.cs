using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _parentCMCameras;
    private GameObject _createdCamera;
    private GameObject _createdParentCMCameras;

    private void OnEnable()
    {
        _createdCamera = Instantiate(_mainCamera, Vector3.zero, Quaternion.identity);
        _createdCamera.name = "MainCamera";

        _createdParentCMCameras = Instantiate(_parentCMCameras, Vector3.zero, Quaternion.identity);
        _createdParentCMCameras.name = "CinemachineCameras";

        _createdParentCMCameras.transform.GetChild(0).name = "BackCamera";
        _createdParentCMCameras.transform.GetChild(1).name = "LeftCannonsCamera";
        _createdParentCMCameras.transform.GetChild(2).name = "RightCannonsCamera";

        List<GameObject> cameras = new List<GameObject>();
        cameras.Add(_createdParentCMCameras.transform.GetChild(0).gameObject);
        cameras.Add(_createdParentCMCameras.transform.GetChild(1).gameObject);
        cameras.Add(_createdParentCMCameras.transform.GetChild(2).gameObject);
        _createdCamera.GetComponent<ShipCameraController>().SetCinemachineCameras(cameras);
    }

    private void OnDisable()
    {
        Destroy(_createdCamera);
        Destroy(_createdParentCMCameras);
    }
}
