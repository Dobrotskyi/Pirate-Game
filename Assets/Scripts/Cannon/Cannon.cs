using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _cannonball;

    private float _cannonShotForce;
    private float _lastShotTime;

    private Ship�haracteristics _ship�haracteristics;
    private Transform _cannonballSpawner;
    private TrajectoryMaker _trajectoryMaker;

    private GameObject _barrel;
    private Quaternion _defaultRotation;
    private GameObject _target;

    public void Shoot()
    {
        if (_ship�haracteristics.CannonballsLeftAmt() > 0 && Time.time - _lastShotTime >= _ship�haracteristics.GetCannonsCooldown())
        {
            GameObject cannonBall = Instantiate(_cannonball, _cannonballSpawner);
            cannonBall.transform.parent = null;
            cannonBall.GetComponent<Rigidbody>().velocity = _ship�haracteristics.TrackVelocity;
            cannonBall.GetComponent<Rigidbody>().AddForce(_cannonballSpawner.forward * _cannonShotForce, ForceMode.Impulse);

            _ship�haracteristics.CannonballFired();
            _lastShotTime = Time.time;
        }
    }

    public void RestoreDefaultPosition()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _defaultRotation, 0.05f);
        _trajectoryMaker.HideLines();
    }

    public void Aim()
    {
        Vector3 direction = _barrel.transform.position - _target.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), 5 * Time.deltaTime);
        _barrel.transform.rotation = Quaternion.Lerp(_barrel.transform.rotation, Quaternion.Euler(rotation.eulerAngles.x, _barrel.transform.rotation.eulerAngles.y, _barrel.transform.rotation.eulerAngles.z), 5 * Time.deltaTime);

        _trajectoryMaker.ShowTrajectory(_cannonballSpawner.position, _cannonShotForce * _cannonballSpawner.forward);
    }

    public void SetTarget(GameObject target) 
    {
        _target = target;
    }

    private void OnEnable()
    {
        _cannonballSpawner = transform.GetChild(0).GetChild(0).transform;
        _ship�haracteristics = transform.parent.parent.GetComponent<Ship�haracteristics>();
        _lastShotTime = -_ship�haracteristics.GetCannonsCooldown();
        _cannonShotForce = _ship�haracteristics.GetCannonsShotForce();

        _barrel = transform.Find("barrel").gameObject;
        _defaultRotation = transform.localRotation;
        _trajectoryMaker = GetComponent<TrajectoryMaker>();
    }

}
