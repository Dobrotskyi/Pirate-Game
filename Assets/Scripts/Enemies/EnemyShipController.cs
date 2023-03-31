using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{

    [Serializable]
    private struct ScreenShakeParameters
    {
        public float Duration;
        public float Strength;
    }

    [SerializeField] private ScreenShakeParameters _screenShakeParameters;
    [SerializeField] private float _delayBetweenShotsInSeconds = 0.3f;
    [SerializeField] private float _reloadTimeInSeconds = 2f;

    private List<Cannon> _leftCannons;
    private List<Cannon> _rightCannons;
    private Transform _target;

    public void StopCannonsAiming()
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.StopAiming();
        foreach (Cannon cannon in _rightCannons)
            cannon.StopAiming();
    }

    public virtual void AimLeftCannons(Vector2 mouseInput)
    {
        mouseInput.y = -mouseInput.y;
        AimCannons(mouseInput, _leftCannons);
    }

    public virtual void AimRightCannons(Vector2 mouseInput)
    {
        mouseInput.x = -mouseInput.x;
        AimCannons(mouseInput, _rightCannons);
    }

    public void ShootLeft()
    {
        StartCoroutine("ShootWithCannons", _leftCannons);
    }

    public void ShootRight()
    {
        StartCoroutine("ShootWithCannons", _rightCannons);
    }

    private IEnumerator ShootWithCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
        {
            if (cannon.CanShoot())
            {
                cannon.Shoot();
                yield return new WaitForSeconds(_delayBetweenShotsInSeconds);
            }
            else
                yield break;
        }
    }

    private void AimCannons(Vector2 mouseInput, List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

    private void SetCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").childCount; i++)
        {
            if (transform.Find("Cannons").GetChild(i).localPosition.x < 0)
            {
                _leftCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
            }
            else
            {
                _rightCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
            }
        }
    }

    private void OnEnable()
    {
        _leftCannons = new List<Cannon>();
        _rightCannons = new List<Cannon>();

        SetCannons();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            _target = other.transform;
            SetNewTargetToCannons();
        }
    }

    private void SetNewTargetToCannons()
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.SetTarget(_target);
    }
}
