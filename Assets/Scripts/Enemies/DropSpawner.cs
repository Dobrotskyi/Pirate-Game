using System.Collections.Generic;
using UnityEngine;
using System;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private List<DropItem> _dropItems;
    [SerializeField] private Vector2 _dropDissplacement = new Vector2(2, 2);

    [Serializable]
    private struct DropItem
    {
        public GameObject Item;
        public int MaxDropAmt;
    }

    public void DropItems(ShipCharacteristics shipCharacteristics)
    {
        Vector3 pos = transform.position;
        foreach (DropItem drop in _dropItems)
        {
            System.Random rand = new System.Random();
            int amt = rand.Next(0, drop.MaxDropAmt + 1);
            for (int i = 0; i < amt; i++)
            {
                GameObject item = Instantiate(drop.Item);
                float newX = UnityEngine.Random.Range(pos.x - _dropDissplacement.x, pos.x + _dropDissplacement.x);
                float newZ = UnityEngine.Random.Range(pos.z - _dropDissplacement.y, pos.z + _dropDissplacement.y);
                item.transform.position = new Vector3(newX, transform.position.y, newZ);
                item.GetComponent<CollectableItem>().GetResourceFromShip(shipCharacteristics, amt);
            }
        }
    }
}
