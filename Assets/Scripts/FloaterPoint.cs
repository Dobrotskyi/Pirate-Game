using UnityEngine;

public class FloaterPoint : MonoBehaviour
{
    private Rigidbody _rb;

    public float depthBeforeSubmerged = 1f;
    public float cubeVolume = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    private Waves _waves;

    private void OnEnable()
    {
        _waves = FindObjectOfType<Waves>();
        Transform baseTransform = transform;
        while (baseTransform.parent != null)
            baseTransform = baseTransform.parent;
        _rb = baseTransform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = _waves.GetHeight(transform.position);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * cubeVolume;
            _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            _rb.AddForce(displacementMultiplier * -_rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            _rb.AddTorque(displacementMultiplier * -_rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

        }
    }
}
