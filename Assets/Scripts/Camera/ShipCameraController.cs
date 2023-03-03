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

    public void SetBackCamera()
    {
        SetAllFalse();
        _cinemachineCameras[0].SetActive(true);
    }

    public void SetCinemachineCameras(List<GameObject> cameras) 
    {
        _cinemachineCameras = cameras;
    }

    private void OnEnable()
    {
        GameObject.Find("Player").GetComponent<PlayerInput>().SetShipCameraController(this);
    }

    private void SetAllFalse()
    {
        foreach (GameObject obj in _cinemachineCameras)
        {
            obj.SetActive(false);
        }
    }
}
