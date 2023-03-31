using System.Collections.Generic;
using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    private List<GameObject> _cinemachineCameras = new List<GameObject>();

    public void SetCameraLeftCannons()
    {
        SetAllFalse();
        _cinemachineCameras[1].SetActive(true);
    }

    public void SetCameraRightCannons()
    {
        SetAllFalse();
        _cinemachineCameras[2].SetActive(true);
    }

    public void SetCameraBehind()
    {
        SetAllFalse();
        _cinemachineCameras[0].SetActive(true);
    }

    public void SetCameras(List<GameObject> cameras) 
    {
        _cinemachineCameras = cameras;
    }

    private void OnEnable()
    {
        GameObject.Find("Player").GetComponent<PlayerOnShipInputHandler>().SetShipCameraController(this);
    }

    private void SetAllFalse()
    {
        foreach (GameObject obj in _cinemachineCameras)
        {
            obj.SetActive(false);
        }
    }
}
