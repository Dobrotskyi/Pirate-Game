using UnityEngine;

public class SmallShipController : ShipController
{
    [SerializeField] private GameObject _targetPrefab;
    private Transform _targetRotationPoint;

    public override void SetLeftCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").transform.childCount; i++)
        {
            _leftCannons.Add(transform.Find("Cannons").transform.GetChild(i).GetComponent<Cannon>());
        }
    }

    public override void SetRightCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").transform.childCount; i++)
        {
            _rightCannons.Add(transform.Find("Cannons").transform.GetChild(i).GetComponent<Cannon>());
        }
    }

    public override void AimLeftCannons(Vector2 mouseInput)
    {
        float xRotation = AngleClamper.ClampAngle(mouseInput.x * 0.5f + _targetRotationPoint.localRotation.eulerAngles.y, 90, -90);
        _targetRotationPoint.localRotation = Quaternion.Euler(0, xRotation, 0);

        float newX = _mainRightTarget.localPosition.x + (mouseInput.y * 10f * Time.deltaTime);
        if (_mainRightTarget.localPosition.x > 0)
        {
            newX = Mathf.Clamp(newX, _targetXLimits[0], _targetXLimits[1]);
        }
        else
        {
            newX = Mathf.Clamp(newX, -_targetXLimits[1], -_targetXLimits[0]);
        }
        _mainRightTarget.localPosition = new Vector3(newX, _mainRightTarget.localPosition.y, _mainRightTarget.localPosition.z);

        foreach (Cannon cannon in _rightCannons)
            cannon.Aim();
    }

    public override void AimRightCannons(Vector2 mouseInput)
    {
        float xRotation = AngleClamper.ClampAngle(mouseInput.x * 0.5f + _targetRotationPoint.localRotation.eulerAngles.y, -90, 90);
        _targetRotationPoint.localRotation = Quaternion.Euler(0, xRotation, 0);

        float newX = _mainRightTarget.localPosition.x + (mouseInput.y * 10f * Time.deltaTime);
        if (_mainRightTarget.localPosition.x > 0)
        {
            newX = Mathf.Clamp(newX, _targetXLimits[0], _targetXLimits[1]);
        }
        else
        {
            newX = Mathf.Clamp(newX, -_targetXLimits[1], -_targetXLimits[0]);
        }
        _mainRightTarget.localPosition = new Vector3(newX, _mainRightTarget.localPosition.y, _mainRightTarget.localPosition.z);

        foreach (Cannon cannon in _rightCannons)
            cannon.Aim();
    }
      
    protected override void OnEnable()
    {
        base.OnEnable();

        Destroy(_mainLeftTarget.gameObject);
        _targetRotationPoint = Instantiate(_targetPrefab, transform).transform;
        _mainRightTarget.parent = _targetRotationPoint;
    }
}
