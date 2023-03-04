using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _parentCMCameras;
    private GameObject _createdCamera;
    private GameObject _createdParentCMCameras;

    private void OnEnable()
    {
        _createdParentCMCameras = Instantiate(_parentCMCameras, Vector3.zero, Quaternion.identity);
        _createdParentCMCameras.name = "CinemachineCameras";
        _createdParentCMCameras.transform.GetChild(0).name = "BackCamera";
        _createdParentCMCameras.transform.GetChild(1).name = "LeftCannonsCamera";
        _createdParentCMCameras.transform.GetChild(2).name = "RightCannonsCamera";

        _createdCamera = Instantiate(_mainCamera, Vector3.zero, Quaternion.identity);
        _createdCamera.name = "MainCamera";
    }

    private void OnDisable()
    {
        Destroy(_createdCamera);
        Destroy(_createdParentCMCameras);
    }
}
