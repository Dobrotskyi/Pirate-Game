using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffect;
    private int _damageAmt = 20;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
        if (collision.transform.TryGetComponent<ITakeDamage>(out ITakeDamage canTakeDamage))
            canTakeDamage.TakeDamage(_damageAmt);
        if (!collision.transform.CompareTag("Cannonball"))
            Destroy(gameObject);
    }
}
