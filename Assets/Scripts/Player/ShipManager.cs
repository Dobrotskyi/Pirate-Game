using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private GameObject _ship;

    public void SetShip()
    {
        GameObject ship = Instantiate(_ship);
        transform.SetParent(ship.transform);
        ship.transform.SetParent(transform);
        ship.transform.localPosition = Vector3.zero;
        ship.transform.localRotation = Quaternion.identity;
    }
    private void Start()
    {
        SetShip();
    }
}
