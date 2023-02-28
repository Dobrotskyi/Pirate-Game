using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    private List<GameObject> _cinemachineCamera = new List<GameObject>();

    private void Start()
    {
        GameObject cameras = GameObject.Find("CinemachineCameras");
        _cinemachineCamera.Add(cameras.transform.Find("BackCamera").gameObject);
        _cinemachineCamera.Add(cameras.transform.Find("LeftCannonsCamera").gameObject);
        _cinemachineCamera.Add(cameras.transform.Find("RightCannonsCamera").gameObject);
    }

    public void SetCameraLeftCannons()
    {
        SetAllFalse();
        _cinemachineCamera[1].SetActive(true);
    }

    public void SetCameraRightCannons()
    {
        SetAllFalse();
        _cinemachineCamera[2].SetActive(true);
    }

    public void SetBackCamera()
    {
        SetAllFalse();
        _cinemachineCamera[0].SetActive(true);
    }

    private void SetAllFalse()
    {
        foreach (GameObject obj in _cinemachineCamera)
        {
            obj.SetActive(false);
        }
    }
}
