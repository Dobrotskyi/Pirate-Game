using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TrajectoryMaker))]
public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _cannonball;
    [SerializeField] private GameObject _afterShotSmokeEffect;
    [SerializeField] private AudioClip _shotAudio;

    protected float _cannonShotForce;
    protected float _cannonballMass;
    protected Transform _target;
    protected Rigidbody _shipRb;
    protected Transform _cannonballSpawner;
    protected TrajectoryMaker _trajectoryMaker;
    protected float _centerOfTargetYOffset = 0f;
    protected GameObject _barrel;

    protected float _cannonRotationSpeed = 5f;

    private float _lastShotTime;
    private ShipCharacteristics _shipCharacteristics;
    private Quaternion _defaultRotation;
    private AudioSource _audioSource;

    public void Shoot()
    {
        GameObject cannonBall = Instantiate(_cannonball, _cannonballSpawner);
        cannonBall.transform.parent = null;
        Rigidbody cannonballRb = cannonBall.GetComponent<Rigidbody>();
        cannonballRb.velocity = _shipRb.velocity;
        cannonballRb.AddForce(_cannonballSpawner.forward * _cannonShotForce, ForceMode.Impulse);

        _shipCharacteristics.CannonballFired();
        _lastShotTime = Time.time;

        GameObject explosion = Instantiate(_afterShotSmokeEffect, _cannonballSpawner.position, _cannonballSpawner.rotation);
        explosion.GetComponent<Rigidbody>().velocity = _shipRb.velocity * 0.7f;
        _audioSource.PlayOneShot(_shotAudio);
    }

    public bool Loaded()
    {
        if (_shipCharacteristics.CannonballsAmt > 0 && Time.time - _lastShotTime >= _shipCharacteristics.GetCannonsCooldown())
            return true;
        else return false;
    }

    public void StopAiming()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _defaultRotation, 0.05f);
        _trajectoryMaker.HideTrajectory();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetTarget(Rigidbody targetRb)
    {
        _target = targetRb.transform;
        _centerOfTargetYOffset = targetRb.centerOfMass.y;
    }

    public void Aim()
    {
        MainPartAim();
        BarrelAim();
    }

    private void MainPartAim()
    {
        Vector3 direction = _barrel.transform.position - GetAimingPoint();
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), _cannonRotationSpeed * Time.deltaTime);

        if (_shipCharacteristics.CannonFOV != -1)
        {
            float xRotation = AngleClamper.ClampAngle(transform.localRotation.eulerAngles.y, _defaultRotation.eulerAngles.y - _shipCharacteristics.CannonFOV / 2, _defaultRotation.eulerAngles.y + _shipCharacteristics.CannonFOV / 2);
            transform.localRotation = Quaternion.Euler(0, xRotation, 0);
        }
    }

    protected virtual Vector3 GetAimingPoint()
    {
        return _target.position;
    }

    protected virtual void BarrelAim()
    {
        Vector3 launchVector = GetLaunchVector();
        Vector3 partToHit = _target.position;
        partToHit.y += _centerOfTargetYOffset;
        float launchAngle = CannonLaunchAngleCounter.GetLaunchAngle(_cannonballSpawner.position, partToHit, launchVector);
        if (float.IsNaN(launchAngle))
            Debug.Log("Target out of reach");

        _barrel.transform.localRotation = Quaternion.Euler(launchAngle, 0, 0);

        _trajectoryMaker.ShowTrajectory(_cannonballSpawner.position, launchVector);
    }

    protected Vector3 GetLaunchVector()
    {
        return _cannonShotForce * _cannonballSpawner.forward / _cannonballMass;
    }

    protected virtual void OnEnable()
    {
        _barrel = transform.Find("barrel").gameObject;
        _cannonballSpawner = _barrel.transform.Find("CannonballSpawner");
        _defaultRotation = transform.localRotation;
        _trajectoryMaker = GetComponent<TrajectoryMaker>();

        Transform shipObj = transform;

        while (shipObj.TryGetComponent<ShipController>(out ShipController shipController) == false && shipObj.parent != null)
            shipObj = shipObj.parent;

        _shipRb = shipObj.GetComponent<Rigidbody>();
        _shipCharacteristics = shipObj.GetComponent<ShipCharacteristics>();
        _lastShotTime = _shipCharacteristics.GetCannonsCooldown();
        _cannonShotForce = _shipCharacteristics.GetCannonsShotForce();

        _audioSource = GetComponent<AudioSource>();
        _cannonballMass = _cannonball.GetComponent<Rigidbody>().mass;
    }
}
