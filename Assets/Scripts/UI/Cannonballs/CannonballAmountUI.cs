using UnityEngine;
using TMPro;

public class CannonballAmountUI : MonoBehaviour
{
    private ShipCharacteristics _currentShipCharacteristics;
    private TextMeshProUGUI _TMP;

    public void UpdateCannonballsAmount(int amt)
    {
        _TMP.text = amt.ToString();
    }

    private void Start()
    {
        _TMP = GetComponent<TextMeshProUGUI>();
        _currentShipCharacteristics = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<ShipCharacteristics>();
        _TMP.text = _currentShipCharacteristics.CannonballsAmt.ToString();
        ShipCharacteristics.OnCannonballsAmtChanged += UpdateCannonballsAmount;
    }

    private void OnDisable()
    {
        ShipCharacteristics.OnCannonballsAmtChanged -= UpdateCannonballsAmount;
    }
}
