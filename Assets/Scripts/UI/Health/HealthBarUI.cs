using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    private void OnEnable()
    {
        PlayerShipCharacteristics.HealthAmtChanged += UpdateHealthInfo;
    }

    private void OnDisable()
    {
        PlayerShipCharacteristics.HealthAmtChanged -= UpdateHealthInfo;
    }

    private void UpdateHealthInfo(int amt)
    {
        if (amt > _healthBar.maxValue)
            _healthBar.maxValue = amt;

        _healthBar.value = amt;
    }
}
