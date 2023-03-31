using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _cannonball;
    [SerializeField] private GameObject _afterShotSmokeEffect;
    [SerializeField] private AudioClip _shotAudio;

    private float _cannonShotForce;
    private float _lastShotTime;

    private ShipCharacteristics _shipCharacteristics;
    private Transform _cannonballSpawner;
    private TrajectoryMaker _trajectoryMaker;

    private GameObject _barrel;
    private Quaternion _defaultRotation;
    private Transform _target;
    private Rigidbody _shipRb;
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

    public bool CanShoot()
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

    public void Aim()
    {
        MainPartAim();
        BarrelAim();
    }

    private void MainPartAim()
    {
        Vector3 direction = _barrel.transform.position - _target.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), 5 * Time.deltaTime);

        if (_shipCharacteristics.CannonFOV != -1)
        {
            float xRotation = AngleClamper.ClampAngle(transform.localRotation.eulerAngles.y, _defaultRotation.eulerAngles.y - _shipCharacteristics.CannonFOV / 2, _defaultRotation.eulerAngles.y + _shipCharacteristics.CannonFOV / 2);
            transform.localRotation = Quaternion.Euler(0, xRotation, 0);
        }
    }

    private void BarrelAim()
    {
        Vector3 launchVector = _cannonShotForce * _cannonballSpawner.forward / _cannonball.GetComponent<Rigidbody>().mass;
        float launchAngle = CannonLaunchAngleCounter.GetLaunchAngle(_cannonballSpawner, _target, launchVector);

        if (float.IsNaN(launchAngle))
            Debug.Log("Target out of reach");
        else
            _barrel.transform.localRotation = Quaternion.Euler(launchAngle, 0, 0);

        _trajectoryMaker.ShowTrajectory(_cannonballSpawner.position, launchVector);
    }

    private void OnEnable()
    {
        _barrel = transform.Find("barrel").gameObject;
        _cannonballSpawner = _barrel.transform.Find("CannonballSpawner");
        _defaultRotation = transform.localRotation;
        _trajectoryMaker = GetComponent<TrajectoryMaker>();

        Transform baseObj = transform;
        while (baseObj.parent != null)
            baseObj = baseObj.parent;
        _shipRb = baseObj.GetComponent<Rigidbody>();
        _shipCharacteristics = baseObj.GetComponent<ShipCharacteristics>();
        _lastShotTime = -_shipCharacteristics.GetCannonsCooldown();
        _cannonShotForce = _shipCharacteristics.GetCannonsShotForce();
        
        _audioSource = GetComponent<AudioSource>();
    }
}
