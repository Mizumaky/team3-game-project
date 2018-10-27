using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerControls_NET : NetworkBehaviour {

    private NavMeshAgent navMeshAgent;
    public float distance = 50f;
    public float rotationSpeed = 10f;

	void Start () {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        navMeshAgent = GetComponent<NavMeshAgent>();
	}

    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<DoubleFocusCamera>().focus = transform;
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            //TODO : need to destroy the NetworkManager on LoadScene...
            //This code does not work
            Destroy(GameObject.Find("LobbyPlayer (Clone)"));
            Destroy(NetworkManager.singleton.gameObject);
            NetworkManager.Shutdown();
            SceneManager.LoadSceneAsync("MainMenu");
        }

        Vector3 groundPositionVector = GetGroundPosition();

        Vector3 direction = (groundPositionVector - transform.position).normalized;
        if (direction != new Vector3(0,0,0)) {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (Input.GetMouseButton(0)) {    
            
            navMeshAgent.SetDestination(groundPositionVector);
            
        }
	}

    Vector3 GetGroundPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance)) {
            return hit.point;
        }
        return GetComponent<Transform>().position;
    }
}
