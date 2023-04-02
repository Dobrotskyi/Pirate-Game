using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPathFinder : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent
    {
        get { return _navMeshAgent; }
    }

    public float TrackVelocity
    {
        private set;
        get;
    }
    internal Transform _target;
    private NavMeshAgent _navMeshAgent;

    private Vector3 _prevPos;
    private Vector3 _currentPos;

    private Transform _destinationTransform;
    private Villages _villages;


    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _villages = FindObjectOfType<Villages>();
        _prevPos = transform.position;
    }

    public IEnumerator Docking()
    {
        yield return new WaitForSeconds(5);
        _navMeshAgent.destination = _villages.GetRandomDock;
        yield break;
    }

    private void AimAtTarget(float rotateToSide)
    {
        _navMeshAgent.enabled = false;
        Vector3 direction = transform.position - _target.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y + rotateToSide, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation.eulerAngles.y, 0), 4 * Time.deltaTime);
        _navMeshAgent.enabled = true;
    }

    private void FixedUpdate()
    {
        TrackVelocity = ((transform.position - _prevPos) * 50).magnitude;
        _prevPos = transform.position;
    }
}
