using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipCharacteristics))]
public class ShipController : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected float _delayBetweenShotsInSeconds = 0.3f;
    protected ShipCharacteristics _shipCharacteristics;
    protected Rigidbody _rb;

    protected List<Cannon> _leftCannons;
    protected List<Cannon> _rightCannons;



    public void Restock()
    {
        _shipCharacteristics.RestockHealthAndCannonballs();
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
        if (AllCannonsAreLoaded(_leftCannons))
            StartCoroutine("ShootWithCannons", _leftCannons);
    }

    public void ShootRight()
    {
        if (AllCannonsAreLoaded(_rightCannons))
            StartCoroutine("ShootWithCannons", _rightCannons);
    }

    public void TakeDamage(int amt)
    {
        _shipCharacteristics.Health -= amt;
    }

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

    protected bool AllCannonsAreLoaded(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
        {
            if (cannon.Loaded() == false)
                return false;
        }
        return true;
    }

    protected virtual IEnumerator ShootWithCannons(List<Cannon> cannons)
    {
        foreach (Cannon cannon in cannons)
        {
            cannon.Shoot();
            yield return new WaitForSeconds(_delayBetweenShotsInSeconds);
        }
    }

    protected virtual void OnEnable()
    {
        _leftCannons = new List<Cannon>();
        _rightCannons = new List<Cannon>();
        _shipCharacteristics = GetComponent<ShipCharacteristics>();

        SetCannons();
    }

    private void FixedUpdate()
    {
        KeepHorizontalVelocityForward();
    }

    private void KeepHorizontalVelocityForward()
    {
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), _rb.velocity);
        horizontalVelocity = forward * horizontalVelocity.magnitude;
        float verticalVelocity = _rb.velocity.y;
        _rb.velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);
    }
}
