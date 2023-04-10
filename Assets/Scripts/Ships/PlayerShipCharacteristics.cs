using UnityEngine;
using System;

public class PlayerShipCharacteristics : ShipCharacteristics
{
    public static new event Action<int> CannonballsAmtChanged;
    public static new event Action<int> HealthAmtChanged;

    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _shipRotationSpeed;

    public override int CannonballsAmt
    {
        get { return base.CannonballsAmt; }
        protected set
        {
            base.CannonballsAmt = value;
            CannonballsAmtChanged?.Invoke(base.CannonballsAmt);
        }
    }

    public override int Health
    {
        get { return base.Health; }
        set
        {
            base.Health = value;
            HealthAmtChanged?.Invoke(Health);
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
