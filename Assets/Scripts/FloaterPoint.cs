using UnityEngine;

public class FloaterPoint : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float depthBeforeSubmerged = 1f;

    [SerializeField] private float cubeVolume = 3f;

    [SerializeField] private int floaterCount = 1;

    [SerializeField] private float waterDrag = 0.99f;

    [SerializeField] private float waterAngularDrag = 0.5f;

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
