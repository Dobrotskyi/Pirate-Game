using UnityEngine;

public class SmallShipController : PlayerShipController
{
    [SerializeField] private GameObject _rotationalPointPrefab;
    private Transform _targetRotationPoint;
    private Vector3 _startingTargetPosition;
    private bool _facingRight = true;

    public override void AimLeftCannons(Vector2 mouseInput)
    {
        if (_facingRight)
        {
            _facingRight = false;
            _targetRotationPoint.localRotation = Quaternion.Euler(0, 180, 0);
        }

        float xRotation = AngleClamper.ClampAngle(mouseInput.x * _sensivity.x + _targetRotationPoint.localRotation.eulerAngles.y, 90, -90);
        _targetRotationPoint.localRotation = Quaternion.Euler(0, xRotation, 0);
        AimCannons(mouseInput);
    }

    public override void AimRightCannons(Vector2 mouseInput)
    {
        if (!_facingRight)
        {
            _targetRotationPoint.localRotation = Quaternion.Euler(0, 0, 0);
            _facingRight = true;
        }

        float xRotation = AngleClamper.ClampAngle(mouseInput.x * _sensivity.x + _targetRotationPoint.localRotation.eulerAngles.y, -90, 90);
        _targetRotationPoint.localRotation = Quaternion.Euler(0, xRotation, 0);
        AimCannons(mouseInput);
    }

    protected void AimCannons(Vector2 mouseInput)
    {
        float newX = _mainRightTarget.localPosition.x + (mouseInput.y * _sensivity.y * Time.deltaTime);
        if (_mainRightTarget.localPosition.x > 0)
        {
            newX = Mathf.Clamp(newX, _targetXLimits[0], _targetXLimits[1]);
        }
        else
        {
            newX = Mathf.Clamp(newX, -_targetXLimits[1], -_targetXLimits[0]);
        }
        _mainRightTarget.localPosition = new Vector3(newX, _mainRightTarget.localPosition.y, _mainRightTarget.localPosition.z);
        _mainRightTarget.position = new Vector3(_mainRightTarget.position.x, 0, _mainRightTarget.position.z);

        foreach (Cannon cannon in _rightCannons)
            cannon.Aim();
    }

    protected override void SetCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").transform.childCount; i++)
        {
            Cannon cannon = transform.Find("Cannons").transform.GetChild(i).GetComponent<Cannon>();
            cannon.SetTarget(_mainRightTarget);
            _rightCannons.Add(cannon);
            _leftCannons.Add(cannon);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _startingTargetPosition = _mainRightTarget.localPosition;
        Destroy(_mainLeftTarget.gameObject);
        _targetRotationPoint = Instantiate(_rotationalPointPrefab, transform).transform;
        _mainRightTarget.parent = _targetRotationPoint;
    }
}
