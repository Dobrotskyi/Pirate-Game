using UnityEngine;

public class Dock : MonoBehaviour
{
    [SerializeField] private GameObject _dockMenu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
            if (_dockMenu.activeSelf == false)
                _dockMenu.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
            _dockMenu.SetActive(false);
    }
}
