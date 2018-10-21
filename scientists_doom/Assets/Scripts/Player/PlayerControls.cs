using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    public float distance = 50f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private float speed;
    private Vector3 lastPosition;

    void Start () {
        speed = 0;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
	}

    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        Vector3 groundPositionVector = GetGroundPosition();

        //print(groundPositionVector);

        Vector3 direction = (groundPositionVector - transform.position).normalized;
        if (direction != new Vector3(0,0,0)) {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (Input.GetMouseButton(0)) {    
            
            navMeshAgent.SetDestination(groundPositionVector);
            
        }

        if (animator != null) {
            
            speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
            lastPosition = transform.position;

            animator.SetFloat("speedParam", speed);
        }
	}

    Vector3 GetGroundPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance)) {
            print(hit);
            
                NavMeshHit hitNavmesh;
                NavMesh.SamplePosition(hit.point, out hitNavmesh, 500, 5);
                return hitNavmesh.position;
            
        }
        return GetComponent<Transform>().position;
    }
}
