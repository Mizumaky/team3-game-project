using UnityEngine;
using UnityEngine.AI;

public class PlayerControls : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    public GameController controller;
    public float distance = 50f;
    public float rotationSpeed = 10f;

    protected Animator animator;
    private float speed;
    private Vector3 lastPosition;

	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speed = 0;
	}

    void Update () {

        if (controller.currentFocusLayer == GameController.FocusLayer.Game) {
            Vector3 groundPositionVector = GetGroundPosition();

            Vector3 direction = (groundPositionVector - transform.position).normalized;
            if (direction != new Vector3(0, 0, 0)) {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            if (Input.GetMouseButton(0)) {
                NavMeshPath path = new NavMeshPath();

                //navMeshAgent.SetDestination(groundPositionVector);
                if (NavMesh.CalculatePath(transform.position, groundPositionVector, NavMesh.AllAreas, path))
                {
                    navMeshAgent.SetPath(path);
                }
                else {
                    Debug.Log("Could not set path");
                }
            }
            else
            {
                navMeshAgent.ResetPath();
            }
        }

        if (animator != null)
        {

            speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
            lastPosition = transform.position;

            animator.SetFloat("speedParam", speed);
        }
	}

    protected Vector3 GetGroundPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance)) {
            if(hit.collider.gameObject.layer == 9)
            {
                NavMeshHit hitNavmesh;
                NavMesh.SamplePosition(hit.point, out hitNavmesh, 500, 5);
                return hitNavmesh.position;
            }
        }
        return GetComponent<Transform>().position;
    }
}
