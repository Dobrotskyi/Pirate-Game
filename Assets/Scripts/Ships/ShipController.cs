using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{


    [SerializeField] protected float[] _targetYLimits = new float[2];
    [SerializeField] protected float[] _targetZLimits = new float[2];


    protected List<Cannon> _leftCannons;
    protected List<Cannon> _rightCannons;

    protected Transform _mainLeftTarget;
    protected Transform _mainRightTarget;

    [SerializeField] private Ship—haracteristics _ship—haracteristics;
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
            Vector3 directionVec = transform.forward * direction;
            _rigidbody.MovePosition(_rigidbody.position + (directionVec * (_ship—haracteristics.Speed * Time.deltaTime)));
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
        AimCannons(mouseInput, _mainLeftTarget, _leftCannons);
    }

    public virtual void AimRightCannons(Vector2 mouseInput)
    {
        mouseInput.x = -mouseInput.x;
        AimCannons(mouseInput, _mainRightTarget, _rightCannons);
    }

    public void AimCannons(Vector2 mouseInput, Transform target, List<Cannon> cannons)
    {
        float newZ = Mathf.Clamp(target.localPosition.z + (mouseInput.x * 10f * Time.deltaTime), _targetZLimits[0], _targetZLimits[1]);
        float newY = Mathf.Clamp(target.localPosition.y + (mouseInput.y * 10f * Time.deltaTime), _targetYLimits[0], _targetYLimits[1]);

        target.localPosition = new Vector3(target.localPosition.x, newY, newZ);

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
