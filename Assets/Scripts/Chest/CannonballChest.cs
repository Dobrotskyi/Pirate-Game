using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballChest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PlayerShip")) 
        {
            int amount = Random.Range(2, 10);
            collision.transform.GetComponentInParent<ShipСharacteristics>().AddCannonballs(amount);
            Destroy(gameObject);
        }
    }
           
}
