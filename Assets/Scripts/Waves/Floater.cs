using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private bool _attachToSurface = false;
    [SerializeField] private Transform[] _floatPoints;
    [SerializeField] private float _rotationSpeed = 0.2f;

    private Rigidbody _rb;
    private Waves _waves;

    private float _waterLine;
    private Vector3[] _waterLinePoints;

    private Vector3 _centerOffset;
    private Vector3 _smoothVectorRotation;
    private Vector3 _targetUp;

    public Vector3 Center { get { return transform.position + _centerOffset; } }

    private void OnEnable()
    {
        _waves = FindObjectOfType<Waves>();

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _waterLinePoints = new Vector3[_floatPoints.Length];

        for (int i = 0; i < _floatPoints.Length; i++)
            _waterLinePoints[i] = _floatPoints[i].position;
        _centerOffset = PhysicsHelper.GetCenter(_waterLinePoints) - transform.position;
    }

    private void FixedUpdate()
    {
        float newWaterLine = 0f;
        bool pointIsUnderWater = false;

        for (int i = 0; i < _floatPoints.Length; i++)
        {
            _waterLinePoints[i] = _floatPoints[i].position;
            _waterLinePoints[i].y = _waves.GetHeight(_floatPoints[i].position);
            newWaterLine += _waterLinePoints[i].y / _floatPoints.Length;
            if (_waterLinePoints[i].y > _floatPoints[i].position.y)
                pointIsUnderWater = true;
        }

        float waterLineDelta = newWaterLine - _waterLine;
        _waterLine = newWaterLine;

        Vector3 gravity = Physics.gravity;
        if (_waterLine > Center.y)
        {
            if (_attachToSurface)
            {
                transform.position = new Vector3(_rb.position.x, _waterLine - _centerOffset.y, _rb.position.z);
            }
            else
            {
                gravity = -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
        }
        _rb.AddForce(gravity * Mathf.Clamp01(Mathf.Abs(_waterLine - Center.y)), ForceMode.Acceleration);

        _targetUp = PhysicsHelper.GetNormal(_waterLinePoints);
        if (pointIsUnderWater)
        {
            _targetUp = Vector3.SmoothDamp(transform.up, _targetUp, ref _smoothVectorRotation, _rotationSpeed);
            _rb.rotation = Quaternion.FromToRotation(transform.up, _targetUp) * _rb.rotation;
        }
    }
}
