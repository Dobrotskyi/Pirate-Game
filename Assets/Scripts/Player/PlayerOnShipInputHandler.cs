using System;
using UnityEngine;

public class PlayerOnShipInputHandler : MonoBehaviour
{
    private PlayerOnShipInput _playerInput;
    private ShipController _shipController;
    private ShipCameraController _shipCameraContoller;

    public void SetShipController(ShipController shipController)
    {
        _shipController = shipController;
    }

    public void SetShipCameraController(ShipCameraController shipCameraController)
    {
        _shipCameraContoller = shipCameraController;
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerOnShipInput>();
        _playerInput.Shoot += ShootEventHandler;
        _playerInput.AimingDirectionChanged += _shipController.StopCannonsAiming;
    }

    private void OnDisable()
    {
        _playerInput.Shoot -= ShootEventHandler;
        _playerInput.AimingDirectionChanged -= _shipController.StopCannonsAiming;
    }

    private void ShootEventHandler()
    {
        if (_playerInput.aimingLeft)
            _shipController.ShootLeft();
        else
            _shipController.ShootRight();
    }

    private void FixedUpdate()
    {
        float mouseVerticalInput = Input.GetAxis("Mouse Y");
        float mouseHorizontalInput = Input.GetAxis("Mouse X");
        Vector2 mouseInput = new Vector2(mouseHorizontalInput, mouseVerticalInput);

        if (_playerInput.aimingLeft)
        {
            _shipCameraContoller.SetCameraLeftCannons();
            _shipController.AimLeftCannons(mouseInput);
        }
        else
        {
            if (_playerInput.aimingRight)
            {
                _shipCameraContoller.SetCameraRightCannons();
                _shipController.AimRightCannons(mouseInput);
            }
            else
            {
                _shipCameraContoller.SetCameraBehind();
            }
        }

        if (_playerInput.goForward)
            _shipController.MoveForward();

        if (_playerInput.rotate)
        {
            float direction = (float)Math.Round(Input.GetAxis("Horizontal"), 3);
            _shipController.Rotate(direction);
        }
    }
}
