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

    private void OnEnable()
    {
        GameObject cmCameras = GameObject.Find("CinemachineCameras");
        for (int i = 0; i < cmCameras.transform.childCount; i++)
            _cinemachineCameras.Add(cmCameras.transform.GetChild(i).gameObject);
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
