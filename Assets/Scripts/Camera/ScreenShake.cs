using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    private float duration = 1f;

    public void Shake()
    {
        StartCoroutine("Shaking");
    }

    private IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = transform.position + Random.insideUnitSphere * strength;
            yield return null;
        }
    }
}
