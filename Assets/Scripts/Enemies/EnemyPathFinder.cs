using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPathFinder : MonoBehaviour
{
    public NavMeshAgent navMeshAgent
    {
        get
        {
            if (_navMeshAgent != null)
                return _navMeshAgent;
            else
                return GetComponent<NavMeshAgent>();
        }
    }

    public float TrackVelocity
    {
        private set;
        get;
    }

    public bool SearchingForNewDock
    {
        private set;
        get;
    }

    private NavMeshAgent _navMeshAgent;

    private Vector3 _prevPos;
    private Villages _villages;

    public void FindNewDock()
    {
        StartCoroutine(Docking());
    }

    public void SetNewDestination(Vector3 point)
    {
        _navMeshAgent.destination = point;
    }

    public bool ShipHasReachedDock()
    {
        if (Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.stoppingDistance)
            return true;

        else return false;
    }

    private IEnumerator Docking()
    {
        SearchingForNewDock = true;
        yield return new WaitForSeconds(5);

        if (_navMeshAgent.isStopped == true)
            _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = _villages.GetRandomDock;
        SearchingForNewDock = false;
        yield break;
    }

    private void FixedUpdate()
    {
        TrackVelocity = ((transform.position - _prevPos) * 50).magnitude;
        _prevPos = transform.position;
    }

    private void OnEnable()
    {
        SearchingForNewDock = false;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _villages = FindObjectOfType<Villages>();
        _prevPos = transform.position;
    }
}
