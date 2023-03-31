using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(ShipCharacteristics))]
public class ShipController : MonoBehaviour
{
    [SerializeField] protected float _delayBetweenShotsInSeconds = 0.3f;
    protected ShipCharacteristics _shipCharacteristics;

    protected List<Cannon> _leftCannons;
    protected List<Cannon> _rightCannons;

    protected virtual void SetCannons()
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

    public void StopCannonsAiming()
    {
        foreach (Cannon cannon in _leftCannons)
            cannon.StopAiming();
        foreach (Cannon cannon in _rightCannons)
            cannon.StopAiming();
    }

    public void ShootLeft()
    {
        StartCoroutine("ShootWithCannons", _leftCannons);
    }

    public void ShootRight()
    {
        StartCoroutine("ShootWithCannons", _rightCannons);
    }

    protected virtual IEnumerator ShootWithCannons(List<Cannon> cannons)
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

    protected virtual void OnEnable()
    {
        _leftCannons = new List<Cannon>();
        _rightCannons = new List<Cannon>();

        SetCannons();
    }
}
