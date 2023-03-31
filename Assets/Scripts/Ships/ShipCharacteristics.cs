using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipCharacteristics : MonoBehaviour
{
    public static event Action<int> OnCannonballsAmtChanged;

    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _shipRotationSpeed;
    [SerializeField] private float _cannonShotForce;
    [SerializeField] private float _cannonsCooldown;
    [SerializeField] private float _cannonFOV;

    private int _cannonBallsAmt;
    private int _maxCannonBallsAmt;

    private List<GameObject> _cannonballs = new List<GameObject>();

    public int CannonballsAmt
    {
        get { return _cannonBallsAmt; }
        private set
        {
            if (value < 0)
                _cannonBallsAmt = 0;
            else
            {
                _cannonBallsAmt = value;
                if (_cannonBallsAmt > _maxCannonBallsAmt)
                    _cannonBallsAmt = _maxCannonBallsAmt;
            }
            OnCannonballsAmtChanged?.Invoke(_cannonBallsAmt);
        }
    }

    public float Speed
    {
        get { return _speed; }
    }

    public float MaxSpeed
    {
        get { return _maxSpeed; }
    }

    public float CannonFOV
    {
        get { return _cannonFOV; }
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
        CannonballsAmt--;
        _cannonballs[CannonballsAmt].SetActive(false);
    }

    public void AddCannonballs(int amount)
    {
        CannonballsAmt += amount;
        for (int i = 0; i < CannonballsAmt; i++)
        {
            if (_cannonballs[i].activeSelf == false)
                _cannonballs[i].SetActive(true);
        }
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
        CannonballsAmt = _maxCannonBallsAmt;
    }
}
