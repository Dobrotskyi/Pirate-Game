using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerShipCharacteristics))]
public class PlayerShipController : ShipController
{
    [SerializeField] GameObject _steeringWheel;
    private Rigidbody _rigidbody;
    private PlayerShipCharacteristics _playerShipCharacteristics;
    private bool _canMoveForward = true;

    public void MoveForward()
    {
        if (_canMoveForward)
        {
            Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
            _rigidbody.AddForceAtPosition(forward * _playerShipCharacteristics.Speed * Time.deltaTime, _steeringWheel.transform.position, ForceMode.Acceleration);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _playerShipCharacteristics.MaxSpeed);
        }
    }

    public void Rotate(float rotationSide)
    {
        Vector3 side = -_steeringWheel.transform.right * rotationSide;
        _rigidbody.AddForceAtPosition(side * Time.deltaTime * _playerShipCharacteristics.RotationSpeed, _steeringWheel.transform.position, ForceMode.Acceleration);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _rigidbody = GetComponent<Rigidbody>();
        _steeringWheel.transform.localPosition = new Vector3(_rigidbody.centerOfMass.x, _rigidbody.centerOfMass.y, _steeringWheel.transform.localPosition.z);
        GameObject.Find("Player").GetComponent<PlayerOnShipInputHandler>().SetShipController(this);
        _playerShipCharacteristics = GetComponent<PlayerShipCharacteristics>();
        base._shipCharacteristics = _playerShipCharacteristics;

    }

    private void FixedUpdate()
    {
        KeepHorizontalVelocityForward();
    }

    private void KeepHorizontalVelocityForward()
    {
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), _rigidbody.velocity);
        horizontalVelocity = forward * horizontalVelocity.magnitude;
        float verticalVelocity = _rigidbody.velocity.y;
        _rigidbody.velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
            _canMoveForward = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
            _canMoveForward = true;
    }
}
