using UnityEngine;

public class SmallShipController : ShipController
{
    public override void SetLeftCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").transform.childCount; i++)
        {
            _leftCannons.Add(transform.Find("Cannons").transform.GetChild(i).GetComponent<Cannon>());
        }
    }

    public override void SetRightCannons()
    {
        for (int i = 0; i < transform.Find("Cannons").transform.childCount; i++)
        {
            _rightCannons.Add(transform.Find("Cannons").transform.GetChild(i).GetComponent<Cannon>());
        }
    }
}
