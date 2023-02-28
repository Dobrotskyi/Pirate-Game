using UnityEngine;

public class SmallShipController : ShipController
{
    [SerializeField] private GameObject _targetPrefab;
    private Transform _target;

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
        AimCannons(mouseInput);
    }

    public override void AimRightCannons(Vector2 mouseInput)
    {
        AimCannons(mouseInput);
    }

    private void AimCannons(Vector2 mouseInput)
    {

        _target.localRotation = Quaternion.Euler(0, mouseInput.x * 0.5f + _target.localRotation.eulerAngles.y, 0);

        float newY = Mathf.Clamp(_mainRightTarget.localPosition.y + (mouseInput.y * 10f * Time.deltaTime), _targetYLimits[0], _targetYLimits[1]);
        _mainRightTarget.localPosition = new Vector3(_mainRightTarget.localPosition.x, newY, _mainRightTarget.localPosition.z);

        foreach (Cannon cannon in _rightCannons)
            cannon.Aim();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Destroy(_mainLeftTarget.gameObject);
        _target = Instantiate(_targetPrefab, transform).transform;
        _mainRightTarget.parent = _target;
    }
}
