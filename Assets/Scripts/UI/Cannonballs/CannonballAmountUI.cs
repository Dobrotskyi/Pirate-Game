using UnityEngine;
using TMPro;

public class CannonballAmountUI : MonoBehaviour
{
    private ShipCharacteristics _currentShipCharacteristics;
    private TextMeshProUGUI _TMP;

    private void UpdateCannonballsAmount(int amt)
    {
        _TMP.text = amt.ToString();
    }

    private void OnEnable()
    {
        _TMP = GetComponent<TextMeshProUGUI>();
        ShipCharacteristics.OnCannonballsAmtChanged += UpdateCannonballsAmount;
    }

    private void OnDisable()
    {
        ShipCharacteristics.OnCannonballsAmtChanged -= UpdateCannonballsAmount;
    }
}
