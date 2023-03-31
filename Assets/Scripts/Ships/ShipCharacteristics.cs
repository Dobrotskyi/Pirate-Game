using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipCharacteristics : MonoBehaviour
{
    [SerializeField] private float _cannonShotForce;
    [SerializeField] private float _cannonsCooldown;
    [SerializeField] private float _cannonFOV;

    private int _cannonBallsAmt;
    private int _maxCannonBallsAmt;

    private List<GameObject> _cannonballs = new List<GameObject>();

    public virtual int CannonballsAmt
    {
        get { return _cannonBallsAmt; }
        protected set
        {
            if (value < 0)
                _cannonBallsAmt = 0;
            else
            {
                _cannonBallsAmt = value;
                if (_cannonBallsAmt > _maxCannonBallsAmt)
                    _cannonBallsAmt = _maxCannonBallsAmt;
            }
        }
    }

    public float CannonFOV
    {
        get { return _cannonFOV; }
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
