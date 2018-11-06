using UnityEngine;
using UnityEngine.AI;

public class PlayerFollowObject : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    
    public float rotationSpeed = 10f;

    public GameObject target;

    protected Animator animator;
    private float speed;
    private Vector3 lastPosition;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        speed = 0;
    }

    private void Update()
    {
        Move();
    }

    //TODO add if state not moving tak at se to nejak resetne do default
    public void StopMoving()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        navMeshAgent.enabled = false;
        if (animator != null)
        {
            animator.SetFloat("speedParam", 0f);

        }
    }
    public void StartMoving()
    {
        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false;
    }

    public void Move()
    {
        Vector3 targetVector = target.transform.position;
        Vector3 direction = (targetVector - transform.position).normalized;
        if (direction != new Vector3(0, 0, 0))
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        NavMeshPath path = new NavMeshPath();

        navMeshAgent.destination = targetVector;
        
        

        if (animator != null)
        {

            speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
            lastPosition = transform.position;

            animator.SetFloat("speedParam", speed);

        }
    }
}