using UnityEngine;

public abstract class CollectableItem: MonoBehaviour
{
    protected int _capacity;
    public abstract void GetResourceFromShip(ShipCharacteristics shipCharacteristics, int sameItemsAmt);
    protected abstract void OnTriggerEnter(Collider other);
}
