using UnityEngine.AI;
using UnityEngine;

public class SiegeMachineController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public Transform target;

    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(target.position);
    }

    void Update(){
    }
}
