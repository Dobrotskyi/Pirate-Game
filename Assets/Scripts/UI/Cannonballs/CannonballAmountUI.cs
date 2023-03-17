using UnityEngine;
using TMPro;

public class CannonballAmountUI : MonoBehaviour
{
    private ShipCharacteristics _currentShipCharacteristics;
    private TextMeshProUGUI _TMP;

    // public void UpdateCannonballsAmount()
    // {
    //     Debug.Log(_currentShipCharacteristics);
    //     if (_currentShipCharacteristics == null)
    //         _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();

    //     _TMP.text = _currentShipCharacteristics.CannonballsLeftAmt().ToString();
    // }

    private void Start()
    {
        // Debug.Log(GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>());
        // _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();
        // _TMP = GetComponent<TextMeshProUGUI>();
        
        _TMP = GetComponent<TextMeshProUGUI>();
        _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();
        _TMP.text = _currentShipCharacteristics.CannonballsLeftAmt().ToString();
    }

    private void FixedUpdate()
    {
        // if (_currentShipCharacteristics == null)
        //     _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();
        //     UpdateCannonballsAmount();
    }
}
