using UnityEngine.AI;
using UnityEngine;

public class SiegeMachineMovement : MonoBehaviour
{
    public GameObject[] axis;

    private float speed;
    private Vector3 lastPosition;
    private NavMeshAgent navAgent;

    void Start(){
        navAgent = GetComponent<NavMeshAgent>();
    }
    public void Update(){
        speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
        lastPosition = transform.position;

        axis[0].transform.Rotate(0, -speed * 7, 0, Space.Self);
        axis[1].transform.Rotate(0, -speed * 7, 0, Space.Self);

        RaycastHit hit;

        int groundLayer = 1 << LayerMask.NameToLayer("Ground");
        float rayLength = 25f;

        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));

        if(Physics.Raycast(ray, out hit, rayLength, groundLayer)) {  
            var targetRot = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            
            
            Debug.DrawRay(transform.position, transform.up, Color.green, 5f);
            Debug.DrawRay(transform.position, transform.forward, Color.blue, 5f);
            Debug.DrawRay(transform.position, navAgent.steeringTarget, Color.yellow, 5f);
        }
    }
}
