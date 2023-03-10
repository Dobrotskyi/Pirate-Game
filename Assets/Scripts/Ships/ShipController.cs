using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] protected float[] _targetXLimits = new float[2];
    [SerializeField] protected float[] _targetYLimits = new float[2];
    [SerializeField] protected float[] _targetZLimits = new float[2];
    [SerializeField] GameObject _steeringWheel;

    protected List<Cannon> _leftCannons;
    protected List<Cannon> _rightCannons;

    protected Transform _mainLeftTarget;
    protected Transform _mainRightTarget;

    [SerializeField] private ShipCharacteristics _ship—haracteristics;
    private Rigidbody _rigidbody;
    private bool _canMoveForward = true;

    public virtual void SetLeftCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").childCount; i++)
        {
            if (transform.Find("Cannons").GetChild(i).localPosition.x < 0)
                _leftCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
        }
    }

    public virtual void SetRightCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").childCount; i++)
        {
            if (transform.Find("Cannons").GetChild(i).position.x > 0)
                _rightCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
        }
    }

    public void MoveForward(float direction)
    {
        if (_canMoveForward)
        {
            Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
            _rigidbody.AddForceAtPosition(forward * _ship—haracteristics.Speed * Time.deltaTime, _steeringWheel.transform.position, ForceMode.Acceleration);
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _ship—haracteristics.MaxSpeed);
        }
    }

    public void Rotate(float rotationSide)
    {
        Vector3 direction = new Vector3(0, transform.position.y + rotationSide, 0);
        Quaternion deltaRotation = Quaternion.Euler(direction * (_ship—haracteristics.RotationSpeed * Time.deltaTime));
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    public void ShootLeft()
    {
        foreach (Cannon canon in _leftCannons)
            canon.Shoot();
    }

    public void ShootRight()
    {
        foreach (Cannon canon in _rightCannons)
            canon.Shoot();
    }

    public void RestoreCannonsPosition()
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.RestoreDefaultPosition();
        foreach (Cannon cannon in _rightCannons)
            cannon.RestoreDefaultPosition();
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

    public void AimCannons(Vector2 mouseInput, Transform target, List<Cannon> cannons)
    {
        float newX = target.localPosition.x + (mouseInput.y * 10f * Time.deltaTime);
        if (target.localPosition.x > 0)
        {
            newX = Mathf.Clamp(newX, _targetXLimits[0], _targetXLimits[1]);
        }
        else
        {
            newX = Mathf.Clamp(newX, -_targetXLimits[1], -_targetXLimits[0]);
        }
        float newZ = Mathf.Clamp(target.localPosition.z + (mouseInput.x * 10f * Time.deltaTime), _targetZLimits[0], _targetZLimits[1]);
        target.localPosition = new Vector3(newX, target.localPosition.y, newZ);

        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

    public virtual void ShootAllCannons()
    {
        int maxCannons = Mathf.Max(_leftCannons.Count, _rightCannons.Count);
        for (int i = 0; i < maxCannons; i++)
        {
            if (_leftCannons[i] == _rightCannons[i])
            {
                _leftCannons[i].Shoot();
                continue;
            }
            if (i < _leftCannons.Count)
            {
                _leftCannons[i].Shoot();
            }
            if (i < _rightCannons.Count)
            {
                _rightCannons[i].Shoot();
            }
        }
    }

    protected virtual void OnEnable()
    {
        _leftCannons = new List<Cannon>();
        _rightCannons = new List<Cannon>();

        _rigidbody = GetComponent<Rigidbody>();

        SetLeftCannons();
        SetRightCannons();

        GameObject.Find("Player").GetComponent<PlayerInput>().SetShipController(this);

        _mainLeftTarget = transform.Find("MainLeftTarget");
        _mainRightTarget = transform.Find("MainRightTarget");

        _steeringWheel.transform.localPosition = new Vector3(_rigidbody.centerOfMass.x, _rigidbody.centerOfMass.y, _steeringWheel.transform.localPosition.z);

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
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        _rigidbody.velocity = forward * _rigidbody.velocity.magnitude;

    }
}
