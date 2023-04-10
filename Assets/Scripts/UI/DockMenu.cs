using UnityEngine;
using UnityEngine.UI;

public class DockMenu : MonoBehaviour
{
    [SerializeField] private Button _restockButton;
    private bool _menuIsOpen = false;
    private PlayerOnShipInputHandler _playerInputHandler;

    public void CloseDockMenu()
    {
        _menuIsOpen = false;
        foreach (Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        _playerInputHandler.enabled = true;
    }

    private void OnEnable()
    {
        GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        _restockButton.onClick.AddListener(playerShip.GetComponent<ShipController>().Restock);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _menuIsOpen == false)
            OpenMenu();
    }

    private void OpenMenu()
    {
        _menuIsOpen = true;
        _playerInputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOnShipInputHandler>();
        _playerInputHandler.enabled = false;
        foreach (Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
    }

    private void OnDisable()
    {
        _restockButton.onClick.RemoveAllListeners();
        _menuIsOpen = false;
    }
}
