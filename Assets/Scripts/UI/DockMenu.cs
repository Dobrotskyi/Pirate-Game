using UnityEngine;
using UnityEngine.UI;

public class DockMenu : MonoBehaviour
{
    [SerializeField] private Button _restockButton;
    private PlayerOnShipInputHandler _playerInputHandler;

    public void HideDockMenu()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        _restockButton.onClick.AddListener(playerShip.GetComponent<ShipController>().Restock);
        _playerInputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOnShipInputHandler>();
        _playerInputHandler.enabled = false;
    }

    private void OnDisable()
    {
        _restockButton.onClick.RemoveAllListeners();
        _playerInputHandler.enabled = true;
    }
}
