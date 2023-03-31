using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShipCharacteristics : ShipCharacteristics
{
    public static event Action<int> OnCannonballsAmtChanged;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _shipRotationSpeed;
    public override int CannonballsAmt
    {
        get { return base.CannonballsAmt; }
        protected set
        {
            base.CannonballsAmt = value;
            OnCannonballsAmtChanged?.Invoke(base.CannonballsAmt);
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

    public float RotationSpeed
    {
        get { return _shipRotationSpeed; }
    }
}
