using System.Collections.Generic;
using UnityEngine;

public class Ship—haracteristics : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _shipRotationSpeed;
    [SerializeField] private float _cannonShotForce;
    [SerializeField] private float _cannonsCooldown;

    private int _cannonBallsAmt;
    private int _maxCannonBallsAmt;

    private List<GameObject> _cannonballs = new List<GameObject>();
    private Vector3 _lastPosition;
    private Vector3 _trackVelocity;

    public float Speed
    {
        get { return _speed; }
    }

    public Vector3 TrackVelocity
    {
        get
        {
            return _trackVelocity;
        }
    }

    public float RotationSpeed
    {
        get { return _shipRotationSpeed; }
    }

    public float GetCannonsCooldown()
    {
        return _cannonsCooldown;
    }

    public void CannonballFired()
    {
        _cannonBallsAmt--;
        _cannonballs[_cannonBallsAmt].SetActive(false);
    }

    public void AddCannonballs(int amount)
    {
        _cannonBallsAmt += amount;

        if (_cannonBallsAmt > _maxCannonBallsAmt)
            _cannonBallsAmt = _maxCannonBallsAmt;

        for (int i = 0; i < _cannonBallsAmt; i++)
        {
            if (_cannonballs[i].activeSelf == false)
                _cannonballs[i].SetActive(true);
        }
    }

    public int CannonballsLeftAmt()
    {
        return _cannonBallsAmt;
    }

    public float GetCannonsShotForce()
    {
        return _cannonShotForce;
    }

    private void OnEnable()
    {
        Transform cannonballs = transform.Find("Cannonballs");
        for (int i = 0; i < cannonballs.childCount; i++)
        {
            _cannonballs.Add(cannonballs.GetChild(i).gameObject);
        }
        _maxCannonBallsAmt = cannonballs.childCount;
        _cannonBallsAmt = _maxCannonBallsAmt;
    }

    private void FixedUpdate()
    {
        _trackVelocity = ((transform.GetComponent<Rigidbody>().position - _lastPosition) * 50);
        _lastPosition = transform.GetComponent<Rigidbody>().position;
    }
}
