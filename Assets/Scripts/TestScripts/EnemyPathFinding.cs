using UnityEngine;
using UnityEngine.AI;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private Transform _destinationTransform;
    private NavMeshAgent _navMeshAgent;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _navMeshAgent.destination = _destinationTransform.position;
    }
}
