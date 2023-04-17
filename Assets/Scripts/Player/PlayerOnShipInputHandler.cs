using System;
using UnityEngine;

public class PlayerOnShipInputHandler : MonoBehaviour
{
    private PlayerOnShipInput _playerInput;
    private PlayerShipController _shipController;
    private ShipCameraController _shipCameraContoller;

    public void SetShipController(PlayerShipController shipController)
    {
        _shipController = shipController;
    }

    public void SetShipCameraController(ShipCameraController shipCameraController)
    {
        _shipCameraContoller = shipCameraController;
    }

    private void OnEnable()
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
        if(_shipController.enabled == false)
            return;

        if (_playerInput.aimingLeft)
            _shipController.ShootLeft();
        else
            _shipController.ShootRight();
    }

    private void FixedUpdate()
    {
        if (_shipController.enabled == false)
            return;

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

        if(_playerInput._closeGame)
            Application.Quit();
    }
}
