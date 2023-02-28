using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraSettings : MonoBehaviour
{
    [SerializeField] private Vector3 _backBody;
    [SerializeField] private Vector3 _leftBody;
    [SerializeField] private Vector3 _rightBody;
    [SerializeField] private Vector3 _aim = Vector3.zero;

    [SerializeField] private GameObject _mainLeftTarget;
    [SerializeField] private GameObject _mainRightTarget;

    private List<CinemachineVirtualCamera> _cmCameras = new List<CinemachineVirtualCamera>(3);

    private void OnEnable()
    {
        GameObject camerasGameObj = GameObject.Find("CinemachineCameras");
        Transform targetTransform = GameObject.Find("Player").transform.Find("CameraTarget");

        for (int i = 0; i < camerasGameObj.transform.childCount; i++)
        {
            _cmCameras.Add(camerasGameObj.transform.GetChild(i).GetComponent<CinemachineVirtualCamera>());
            _cmCameras[i].m_Follow = targetTransform;
        }

        _cmCameras[0].LookAt = targetTransform;
        _cmCameras[0].GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _backBody;

        _cmCameras[1].LookAt = _mainLeftTarget.transform;
        _cmCameras[1].GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _leftBody;

        _cmCameras[2].LookAt = _mainRightTarget.transform;
        _cmCameras[2].GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _rightBody;

        if (_aim != Vector3.zero)
            _cmCameras[0].GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = _aim;
    }
}
