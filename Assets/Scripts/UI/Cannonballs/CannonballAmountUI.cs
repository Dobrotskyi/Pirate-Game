using UnityEngine;
using TMPro;

public class CannonballAmountUI : MonoBehaviour
{
    private ShipCharacteristics _currentShipCharacteristics;
    private TextMeshProUGUI _TMP;

    public void UpdateCannonballsAmount()
    {
        Debug.Log("Here");
        if (_currentShipCharacteristics == null)
            _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();

        _TMP.text = _currentShipCharacteristics.CannonballsAmt.ToString();
    }

    private void OnEnable()
    {
        _TMP = GetComponent<TextMeshProUGUI>();
        _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();
        Debug.Log( _currentShipCharacteristics.CannonballsAmt.ToString());
        _TMP.text = _currentShipCharacteristics.CannonballsAmt.ToString();
        _currentShipCharacteristics.OnCannonballsAmtChanged += UpdateCannonballsAmount;
    }

    private void OnDisable()
    {
        _currentShipCharacteristics.OnCannonballsAmtChanged -= UpdateCannonballsAmount;
    }
}
