using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPathFinding : MonoBehaviour
{
    private Transform _destinationTransform;
    private NavMeshAgent _navMeshAgent;
    private Villages _villages;
    private bool _destinationReached = true;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _villages = FindObjectOfType<Villages>();
        StartCoroutine(Docking());
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _navMeshAgent.destination) <= _navMeshAgent.stoppingDistance && _destinationReached == false)
        {
            _destinationReached = true;
            StartCoroutine(Docking());
        }
    }

    private IEnumerator Docking()
    {
        yield return new WaitForSeconds(5);

        _navMeshAgent.destination = _villages.GetRandomDock;
        _destinationReached = false;
        yield break;
    }
}
