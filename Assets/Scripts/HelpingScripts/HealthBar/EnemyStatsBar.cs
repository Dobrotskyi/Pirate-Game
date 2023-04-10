using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _cannonballsBar;

    [Header("Can leave it empty")]
    [SerializeField] private ShipCharacteristics _characteristics;

    private void OnEnable()
    {
        if (_characteristics == null)
            FindCharacteristicsInParents();

        _healthBar.maxValue = _characteristics.Health;
        _healthBar.value = _characteristics.Health;
        _cannonballsBar.maxValue = _characteristics.CannonballsAmt;
        _cannonballsBar.value = _characteristics.CannonballsAmt;

        _characteristics.HealthAmtChanged += UpdateHealthBarInfo;
        _characteristics.CannonballsAmtChanged += UpdateCannonballsBarInfo;
    }

    private void OnDisable()
    {
        _characteristics.HealthAmtChanged -= UpdateHealthBarInfo;
        _characteristics.CannonballsAmtChanged -= UpdateCannonballsBarInfo;
    }

    private void FindCharacteristicsInParents()
    {
        Transform current = transform;
        while (current.gameObject.TryGetComponent<ShipCharacteristics>(out _characteristics) == false && current.parent != null)
            current = current.parent;
    }

    private void UpdateHealthBarInfo(int amt)
    {
        _healthBar.value = amt;
    }

    private void UpdateCannonballsBarInfo(int amt)
    {
        _cannonballsBar.value = amt;
    }
}
