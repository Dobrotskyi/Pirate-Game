using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static event Action<float, float> OnCannonFired;

    [SerializeField] protected float[] _targetXLimits = new float[2];
    [SerializeField] protected float[] _targetZLimits = new float[2];
    [SerializeField] GameObject _steeringWheel;
    [SerializeField] private float _delayBetweenShotsInSeconds = 0.3f;
    [SerializeField] private ShipCharacteristics _shipCharacteristics;

    [Serializable]
    private struct ScreenShakeParameters
    {
        public float Duration;
        public float Strength;
    }

    [SerializeField] private ScreenShakeParameters _screenShakeParameters;
    [SerializeField] private Vector2 _sensivity = new Vector2(10f, 100f);

    protected List<Cannon> _leftCannons;
    protected List<Cannon> _rightCannons;

    protected Transform _mainLeftTarget;
    protected Transform _mainRightTarget;

    private Rigidbody _rigidbody;
    private bool _canMoveForward = true;

    protected virtual void SetCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").childCount; i++)
        {
            if (transform.Find("Cannons").GetChild(i).localPosition.x < 0)
            {
                _leftCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
                _leftCannons[_leftCannons.Count - 1].SetTarget(_mainLeftTarget.gameObject);
            }
            else
            {
                _rightCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
                _rightCannons[_rightCannons.Count - 1].SetTarget(_mainRightTarget.gameObject);
            }
        }
    }

    public void MoveForward()
    {
        if (_canMoveForward)
        {
            Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
            _rigidbody.AddForceAtPosition(forward * _shipCharacteristics.Speed * Time.deltaTime, _steeringWheel.transform.position, ForceMode.Acceleration);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _shipCharacteristics.MaxSpeed);
        }
    }

    public void Rotate(float rotationSide)
    {
        Vector3 side = -_steeringWheel.transform.right * rotationSide;
        _rigidbody.AddForceAtPosition(side * Time.deltaTime * _shipCharacteristics.RotationSpeed, _steeringWheel.transform.position, ForceMode.Acceleration);
    }

    public void StopCannonsAiming()
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.StopAiming();
        foreach (Cannon cannon in _rightCannons)
            cannon.StopAiming();
    }

    public virtual void AimLeftCannons(Vector2 mouseInput)
    {
        mouseInput.y = -mouseInput.y;
        AimCannons(mouseInput, _mainLeftTarget, _leftCannons);
    }

    public virtual void AimRightCannons(Vector2 mouseInput)
    {
        mouseInput.x = -mouseInput.x;
        AimCannons(mouseInput, _mainRightTarget, _rightCannons);
    }

    public void ShootLeft()
    {
        StartCoroutine("ShootWithCannons", _leftCannons);
    }

    public void ShootRight()
    {
        StartCoroutine("ShootWithCannons", _rightCannons);
    }

    protected IEnumerator ShootWithCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
        {
            if (cannon.CanShoot())
            {
                cannon.Shoot();
                OnCannonFired?.Invoke(_screenShakeParameters.Duration, _screenShakeParameters.Strength);
                yield return new WaitForSeconds(_delayBetweenShotsInSeconds);
            }
            else
                yield break;
        }
    }

    protected virtual void OnEnable()
    {
        _leftCannons = new List<Cannon>();
        _rightCannons = new List<Cannon>();

        _rigidbody = GetComponent<Rigidbody>();

        GameObject.Find("Player").GetComponent<PlayerInput>().SetShipController(this);
        GameObject.Find("Player").GetComponent<PlayerOnShipInputHandler>().SetShipController(this);

        _mainLeftTarget = transform.Find("MainLeftTarget");
        _mainRightTarget = transform.Find("MainRightTarget");

        SetCannons();

        _steeringWheel.transform.localPosition = new Vector3(_rigidbody.centerOfMass.x, _rigidbody.centerOfMass.y, _steeringWheel.transform.localPosition.z);
    }

    private void AimCannons(Vector2 mouseInput, Transform target, List<Cannon> cannons)
    {
        float newX = target.localPosition.x + (mouseInput.y * _sensivity.y * Time.deltaTime);
        if (target.localPosition.x > 0)
        {
            newX = Mathf.Clamp(newX, _targetXLimits[0], _targetXLimits[1]);
        }
        else
        {
            newX = Mathf.Clamp(newX, -_targetXLimits[1], -_targetXLimits[0]);
        }
        float newZ = Mathf.Clamp(target.localPosition.z + (mouseInput.x * _sensivity.x * Time.deltaTime), _targetZLimits[0], _targetZLimits[1]);
        target.localPosition = new Vector3(newX, target.localPosition.y, newZ);
        target.position = new Vector3(target.position.x, 0, target.position.z);

        foreach (Cannon cannon in cannons)
            cannon.Aim();
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
}
