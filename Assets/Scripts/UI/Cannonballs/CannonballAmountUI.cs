using UnityEngine;
using TMPro;

public class CannonballAmountUI : MonoBehaviour
{
    private TextMeshProUGUI _TMP;

    private void UpdateCannonballsAmount(int amt)
    {
        _TMP.text = amt.ToString();
    }

    private void OnEnable()
    {
        _TMP = GetComponent<TextMeshProUGUI>();
        PlayerShipCharacteristics.OnCannonballsAmtChanged += UpdateCannonballsAmount;
    }

    private void OnDisable()
    {
        PlayerShipCharacteristics.OnCannonballsAmtChanged -= UpdateCannonballsAmount;
    }
}
