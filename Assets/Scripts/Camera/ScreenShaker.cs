using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    public void Shake(float duration, float strength)
    {
        StartCoroutine(Shaking(duration, strength));
    }

    private IEnumerator Shaking(float duration, float strength)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float step = curve.Evaluate(elapsedTime / duration);
            transform.position = transform.position + Random.insideUnitSphere * step * strength;
            yield return null;
        }
    }

    private void OnEnable()
    {
        FindObjectOfType<ShipController>().OnCannonFired += Shake;
    }

    private void OnDisable()
    {
        FindObjectOfType<ShipController>().OnCannonFired -= Shake;
    }
}
