using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _cannonball;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private AudioClip _shotAudio;

    private float _cannonShotForce;
    private float _lastShotTime;

    private ShipCharacteristics _shipCharacteristics;
    private Transform _cannonballSpawner;
    private TrajectoryMaker _trajectoryMaker;

    private GameObject _barrel;
    private Quaternion _defaultRotation;
    private Transform _target;

    public void Shoot()
    {
        GameObject cannonBall = Instantiate(_cannonball, _cannonballSpawner);
        cannonBall.transform.parent = null;
        cannonBall.GetComponent<Rigidbody>().velocity = _shipCharacteristics.TrackVelocity;
        cannonBall.GetComponent<Rigidbody>().AddForce(_cannonballSpawner.forward * _cannonShotForce, ForceMode.Impulse);

        _shipCharacteristics.CannonballFired();
        _lastShotTime = Time.time;

        GameObject explosion = Instantiate(_explosionEffect, _cannonballSpawner.position, _cannonballSpawner.rotation);
        explosion.GetComponent<Rigidbody>().velocity = _shipCharacteristics.TrackVelocity;
        GetComponent<AudioSource>().PlayOneShot(_shotAudio);
    }

    public bool CanShoot()
    {
        if (_shipCharacteristics.CannonballsLeftAmt() > 0 && Time.time - _lastShotTime >= _shipCharacteristics.GetCannonsCooldown())
            return true;
        else return false;
    }

    public void RestoreDefaultPosition()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _defaultRotation, 0.05f);
        _trajectoryMaker.HideTrajectory();
    }

    public void SetTarget(GameObject target)
    {
        _target = target.transform;
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
        _cannonballSpawner = transform.GetChild(0).GetChild(0).transform;
        _shipCharacteristics = transform.parent.parent.GetComponent<ShipCharacteristics>();
        _lastShotTime = -_shipCharacteristics.GetCannonsCooldown();
        _cannonShotForce = _shipCharacteristics.GetCannonsShotForce();

        _barrel = transform.Find("barrel").gameObject;
        _defaultRotation = transform.localRotation;
        _trajectoryMaker = GetComponent<TrajectoryMaker>();
    }
}
