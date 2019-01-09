using System.Collections;
using UnityEngine.AI;
using UnityEngine;

public class ScientistPatroller : MonoBehaviour
{
    public GameObject patrolPointParent;
    
    private int points;
    private NavMeshAgent agent;
    private float speed;
    private Animator animator;
    private Vector3 lastPosition;

    private bool patrol;

    void Start()
    {
        points = patrolPointParent.transform.childCount;
        agent = GetComponent<NavMeshAgent>();
        speed = 0;
        animator = GetComponentInChildren<Animator>();
        patrol = false;
        StartPatrol();
    }

    public void StartPatrol(){
        patrol = true;
        StartCoroutine(GoToPoint());
    }

    public void StopPatrol(){
        patrol = false;
        StopAllCoroutines();
    }

    private IEnumerator GoToPoint(){
        while(patrol){
            agent.SetDestination(patrolPointParent.transform.GetChild((int)Random.Range(0, points-1)).transform.position);
            yield return new WaitForSeconds(Random.Range(4f, 10f));
        }
    }

    void Update(){
        if (animator != null){

            speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
            lastPosition = transform.position;

            animator.SetFloat("speedParam", speed);

        }
    }
}
