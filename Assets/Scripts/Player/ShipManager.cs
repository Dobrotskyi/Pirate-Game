using UnityEngine;
using System.Collections.Generic;

public class ShipManager : MonoBehaviour
{
    public static bool FinalShip
    {
        private set;
        get;
    }

    [SerializeField] private List<GameObject> _ships;
    [SerializeField] private int _currentShipIndex = 2;

    public void UpgradeShip()
    {
        if (_currentShipIndex + 1 >= _ships.Count)
            return;
        GameObject currentShip = transform.parent.gameObject;
        transform.parent = null;
        Destroy(currentShip);
        CreateShip(++_currentShipIndex);
    }

    private void Awake()
    {
        CreateShip(_currentShipIndex);
    }

    private void CreateShip(int index)
    {
        GameObject ship = Instantiate(_ships[index]);
        transform.SetParent(ship.transform);
        ship.transform.SetParent(transform);
        ship.transform.localPosition = transform.position;
        ship.transform.rotation = transform.rotation;
        transform.localPosition = Vector3.zero;

        CheckIfShipIsFinal();
    }

    private void CheckIfShipIsFinal()
    {
        if(_currentShipIndex == _ships.Count - 1)
            FinalShip = true;
    }
}
