using UnityEngine;

public class Dock : MonoBehaviour
{
    [SerializeField] private GameObject _dockMenu;
    private bool _playerInDock;

    private void Update()
    {
        if(_playerInDock)
            if(Input.GetKeyDown(KeyCode.E) && _dockMenu.activeSelf == false)
                _dockMenu.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerShip"))
        {
            _playerInDock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerShip"))
        {
            _playerInDock = false;
            _dockMenu.SetActive(false);
        }
    }
}
