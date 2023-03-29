using System;
using UnityEngine;

[RequireComponent(typeof(PlayerOnShipInputHandler))]
public class PlayerOnShipInput : MonoBehaviour
{
    internal event Action Shoot;
    internal event Action AimingDirectionChanged;
    internal bool aimingLeft = false;
    internal bool setCameraLeft = false;
    internal bool aimingRight = false;
    internal bool goForward = false;
    internal bool rotate = false;

    private enum possibleAimingDirection
    {
        Left,
        Right,
        None
    }

    private possibleAimingDirection _lastAimingDirection = possibleAimingDirection.None;
    private possibleAimingDirection _currentAimingDirection;

    private void Update()
    {
        CheckMouseInput();
        CheckKeyboardInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            aimingLeft = true;
            aimingRight = false;
            _currentAimingDirection = possibleAimingDirection.Left;
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                aimingRight = true;
                aimingLeft = false;
                _currentAimingDirection = possibleAimingDirection.Right;
            }
            else
            {
                aimingLeft = false;
                aimingRight = false;
                _currentAimingDirection = possibleAimingDirection.None;
            }
        }

        if (_lastAimingDirection != _currentAimingDirection)
        {
            AimingDirectionChanged?.Invoke();
            _lastAimingDirection = _currentAimingDirection;
        }
    }

    private void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (aimingLeft || aimingRight)
            {
                Shoot?.Invoke();
            }
        }

        if (Input.GetAxis("Vertical") > 0)
            goForward = true;
        else
            goForward = false;

        if (Math.Round(Input.GetAxis("Horizontal"), 3) != 0)
            rotate = true;
        else
            rotate = false;

    }
}
