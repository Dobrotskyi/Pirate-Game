using UnityEngine;

public class DockHighlight : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("PlayerShip"))
            _meshRenderer.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.CompareTag("PlayerShip"))
            _meshRenderer.enabled = true;
    }
}
