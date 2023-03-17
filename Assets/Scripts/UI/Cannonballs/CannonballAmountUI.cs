using UnityEngine;
using TMPro;

public class CannonballAmountUI: MonoBehaviour
{
    private ShipCharacteristics _currentShipCharacteristics;
    private TextMeshProUGUI _TMP;
    
    public void UpdateCannonballsAmount()
    {
        if(_currentShipCharacteristics == null)
             _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();
        _TMP.text = _currentShipCharacteristics.CannonballsLeftAmt().ToString();
    }

    private void Start()
    {
        _TMP = GetComponent<TextMeshProUGUI>();
    }
}
