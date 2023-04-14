using UnityEngine;

public class CannonballBarrel : CollectableItem
{
    [SerializeField] private float _lifeTime = 5f;
    public override void GetResourceFromShip(ShipCharacteristics shipCharacteristics, int otherBarrelsAmt)
    {
        base._capacity = shipCharacteristics.CannonballsAmt / otherBarrelsAmt;
        if(_capacity == 0)
            Destroy(this.gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("PlayerShip"))
        {
            other.transform.GetComponentInParent<ShipCharacteristics>().AddCannonballs(base._capacity);
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Destroy(this.gameObject, _lifeTime);
    }
}
