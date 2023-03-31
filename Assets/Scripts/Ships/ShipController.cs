using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(ShipCharacteristics))]
public class ShipController : MonoBehaviour
{
    public static event Action<float, float> OnCannonFired;

    [SerializeField] protected float[] _targetXLimits = new float[2];
    [SerializeField] protected float[] _targetZLimits = new float[2];
    [SerializeField] protected Vector2 _sensivity = new Vector2(10f, 100f);
    [SerializeField] private float _delayBetweenShotsInSeconds = 0.3f;
    protected ShipCharacteristics _shipCharacteristics;

    [Serializable]
    private struct ScreenShakeParameters
    {
        public float Duration;
        public float Strength;
    }

    [SerializeField] private ScreenShakeParameters _screenShakeParameters;


    protected List<Cannon> _leftCannons;
    protected List<Cannon> _rightCannons;

    protected Transform _mainLeftTarget;
    protected Transform _mainRightTarget;

    protected virtual void SetCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").childCount; i++)
        {
            if (transform.Find("Cannons").GetChild(i).localPosition.x < 0)
            {
                _leftCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
                _leftCannons[_leftCannons.Count - 1].SetTarget(_mainLeftTarget);
            }
            else
            {
                _rightCannons.Add(transform.Find("Cannons").GetChild(i).GetComponent<Cannon>());
                _rightCannons[_rightCannons.Count - 1].SetTarget(_mainRightTarget);
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

    public virtual void AimLeftCannons(Vector2 mouseInput)
    {
        mouseInput.y = -mouseInput.y;
        AimCannons(mouseInput, _mainLeftTarget, _leftCannons);
    }

    public virtual void AimRightCannons(Vector2 mouseInput)
    {
        mouseInput.x = -mouseInput.x;
        AimCannons(mouseInput, _mainRightTarget, _rightCannons);
    }

    public void ShootLeft()
    {
        StartCoroutine("ShootWithCannons", _leftCannons);
    }

    public void ShootRight()
    {
        StartCoroutine("ShootWithCannons", _rightCannons);
    }

    protected IEnumerator ShootWithCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
        {
            if (cannon.CanShoot())
            {
                cannon.Shoot();
                OnCannonFired?.Invoke(_screenShakeParameters.Duration, _screenShakeParameters.Strength);
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

        _mainLeftTarget = transform.Find("MainLeftTarget");
        _mainRightTarget = transform.Find("MainRightTarget");

        SetCannons();
    }

    private void AimCannons(Vector2 mouseInput, Transform target, List<Cannon> cannons)
    {
        float newX = target.localPosition.x + (mouseInput.y * _sensivity.y * Time.deltaTime);
        if (target.localPosition.x > 0)
        {
            newX = Mathf.Clamp(newX, _targetXLimits[0], _targetXLimits[1]);
        }
        else
        {
            newX = Mathf.Clamp(newX, -_targetXLimits[1], -_targetXLimits[0]);
        }
        float newZ = Mathf.Clamp(target.localPosition.z + (mouseInput.x * _sensivity.x * Time.deltaTime), _targetZLimits[0], _targetZLimits[1]);
        target.localPosition = new Vector3(newX, target.localPosition.y, newZ);
        target.position = new Vector3(target.position.x, 0, target.position.z);

        foreach (Cannon cannon in cannons)
            cannon.Aim();
    }

}
