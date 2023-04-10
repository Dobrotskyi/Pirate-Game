using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private ShipCharacteristics _characteristics;

    private void OnEnable()
    {
        if(_characteristics == null)
            FindCharacteristics();

        _healthBar.maxValue = _characteristics.Health;
        _healthBar.value = _characteristics.Health;
        _characteristics.HealthAmtChanged += UpdateHealthBarInfo;
    }

    private void FindCharacteristics()
    {
        Transform current = transform;
        while (current.gameObject.TryGetComponent<ShipCharacteristics>(out _characteristics) == false && current.parent != null)
            current = current.parent;
    }

    private void OnDisable()
    {
        _characteristics.HealthAmtChanged -= UpdateHealthBarInfo;
    }

    private void UpdateHealthBarInfo(int amt)
    {
        _healthBar.value = amt;
    }
}
