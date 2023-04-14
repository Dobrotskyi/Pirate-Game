using UnityEngine;
using System.Collections;

public class ShipExplosionOnSinking : MonoBehaviour
{
    [SerializeField] private GameObject _effect;
    [SerializeField] private int _maxDelay = 8;

    public void PlayExplosionEffect()
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].isTrigger == false)
                StartCoroutine(PlayExplosion(colliders[i].bounds.center));
        }
    }

    private IEnumerator PlayExplosion(Vector3 pos)
    {
        while (true)
        {
            int waitForSeconds = Random.Range(0, _maxDelay);
            yield return new WaitForSeconds(waitForSeconds);
            GameObject effect = Instantiate(_effect, pos, transform.rotation);
        }
    }
}
