using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DockMenu : MonoBehaviour
{
    [SerializeField] private Button _restockButton;
    [SerializeField] private int _priceForUpgrade = 2;
    [SerializeField] private int _priceIncrease = 2;
    private bool _menuIsOpen = false;
    private PlayerOnShipInputHandler _playerInputHandler;
    private int _score;

    public void CloseDockMenu()
    {
        _menuIsOpen = false;
        foreach (Transform child in transform)
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        _playerInputHandler.enabled = true;
    }

    public void UpgradeShip()
    {
        GameObject player = _playerInputHandler.gameObject;
        player.GetComponent<ShipManager>().UpgradeShip();
        _priceForUpgrade += _priceIncrease;
    }

    private void OnEnable()
    {
        GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        _restockButton.onClick.AddListener(playerShip.GetComponent<ShipController>().Restock);
        _score = Convert.ToInt32(GameObject.FindGameObjectWithTag("ScoreCounter").GetComponent<TextMeshProUGUI>().text);
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

        Button upgradeButton = transform.Find("UpgradeShipButton").GetComponent<Button>();
        TextMeshProUGUI upgradeButtonText = transform.Find("UpgradeShipButton").GetChild(0).GetComponent<TextMeshProUGUI>();
        Debug.Log(_score);
        Debug.Log(_priceForUpgrade);
        Debug.Log(_score - _priceForUpgrade >= 0);
        if (_score - _priceForUpgrade >= 0)
        {
            upgradeButton.enabled = true;
            upgradeButtonText.text = "Upgrade ship, cost is: " + _priceForUpgrade;
        }
        else
        {
            upgradeButton.enabled = false;
            upgradeButtonText.text = "Not enough score to upgrade, price is: " + _priceForUpgrade;
        }
    }

    private void OnDisable()
    {
        _restockButton.onClick.RemoveAllListeners();
        _menuIsOpen = false;
    }
}
