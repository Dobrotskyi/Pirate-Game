using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    private void OnEnable()
    {
        PlayerShipCharacteristics.HealthAmtChanged += UpdateHealthInfo;
        PlayerShipCharacteristics.NewShipSpawned += SetMaxHealth;
    }

    private void OnDisable()
    {
        PlayerShipCharacteristics.HealthAmtChanged -= UpdateHealthInfo;
        PlayerShipCharacteristics.NewShipSpawned -= SetMaxHealth;

    }

    private void UpdateHealthInfo(int amt)
    {
        if (amt > _healthBar.maxValue)
            _healthBar.maxValue = amt;

        _healthBar.value = amt;
    }

    private void SetMaxHealth(int maxHealth)
    {
        _healthBar.maxValue = maxHealth;
        UpdateHealthInfo(maxHealth);
    }
}
