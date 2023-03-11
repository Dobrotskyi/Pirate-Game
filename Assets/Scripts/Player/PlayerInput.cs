using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private ShipCameraController _shipCameraContoller;
    private ShipController _shipController;

    public void SetShipController(ShipController shipController)
    {
        _shipController = shipController;
    }

    public void SetShipCameraController(ShipCameraController shipCameraController)
    {
        _shipCameraContoller = shipCameraController;
    }

    private void FixedUpdate()
    {
        float verticalInput = (float)Math.Round(Input.GetAxis("Vertical"), 3);
        float horizontalInput = (float)Math.Round(Input.GetAxis("Horizontal"), 3);

        if (verticalInput > 0)
            _shipController.MoveForward(verticalInput);

        if (horizontalInput != 0)
            _shipController.Rotate(horizontalInput);

        if ((!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) == true)
        {
            _shipCameraContoller.SetBackCamera();
            _shipController.RestoreCannonsPosition();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _shipCameraContoller.SetCameraLeftCannons();
        }

        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                _shipCameraContoller.SetCameraRightCannons();
            }
        }

        float mouseVerticalInput = Input.GetAxis("Mouse Y");
        float mouseHorizontalInput = Input.GetAxis("Mouse X");
        Vector2 mouseInput = new Vector2(mouseHorizontalInput, mouseVerticalInput);

        if (Input.GetMouseButton(0))
        {
            _shipController.AimLeftCannons(mouseInput);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _shipController.ShootLeft();
            }
        }

        if (Input.GetMouseButton(1))
        {
            _shipController.AimRightCannons(mouseInput);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _shipController.ShootRight();
            }
        }
    }
}
