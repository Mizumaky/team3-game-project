using UnityEngine;
using UnityEngine.AI;

public class EnemyControls : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    public Transform castle;
    public GameObject player;
    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(castle.position);

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
