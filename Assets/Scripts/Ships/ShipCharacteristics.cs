using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipCharacteristics : MonoBehaviour
{
    public event Action<int> HealthAmtChanged;
    public event Action<int> CannonballsAmtChanged;

    [SerializeField] private float _cannonShotForce;
    [SerializeField] private float _cannonsCooldown;
    [SerializeField] private float _cannonFOV;
    [SerializeField] private int _maxHealth = 50;
    private int _health;

    private int _cannonballsAmt;
    private int _maxCannonBallsAmt;
    private bool _noCannonballs = false;

    private List<GameObject> _cannonballs = new List<GameObject>();

    public virtual int CannonballsAmt
    {
        get { return _cannonballsAmt; }
        protected set
        {
            if (value <= 0)
            {
                _cannonballsAmt = 0;
                _noCannonballs = true;
            }
            else
            {
                _cannonballsAmt = value;
                if (_cannonballsAmt > _maxCannonBallsAmt)
                    _cannonballsAmt = _maxCannonBallsAmt;
                _noCannonballs = false;
            }
            
            CannonballsAmtChanged?.Invoke(_cannonballsAmt);
        }
    }

    public virtual int Health
    {
        get{return _health;}
        set
        {
            _health = value;
            HealthAmtChanged?.Invoke(_health);
            if(_health <= 0)
            {
                Destroy(gameObject);
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

    public bool NoCannonballs
    {
        get { return _noCannonballs; }
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

    public void RestoreHealthAndCannonballs()
    {
        CannonballsAmt = _maxCannonBallsAmt;
        Health = _maxHealth;
    }

    protected virtual void OnEnable()
    {
        Health = _maxHealth;
        Transform cannonballs = transform.Find("Cannonballs");
        for (int i = 0; i < cannonballs.childCount; i++)
        {
            _cannonballs.Add(cannonballs.GetChild(i).gameObject);
        }
        _maxCannonBallsAmt = cannonballs.childCount;
        CannonballsAmt = _maxCannonBallsAmt;
    }
}
