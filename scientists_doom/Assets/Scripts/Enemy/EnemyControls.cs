using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyControls : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    public Transform castle;
    private Transform target;
    private NavMeshPath path;
    private bool followPlayer;
    private float distanceToFollowPlayer = 7f;

    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        target = castle;
        followPlayer = false;
        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
            navMeshAgent.SetPath(path);
        else
            Debug.Log("Enemy could not set path");
    }

    private void OnCollisionEnter(Collision other) //colision with prijectile
    {
        if (other.gameObject.layer == 11) // 11. layer hit enemies
        {
            target = other.collider.GetComponentInChildren<Projectile>().casterTransform;
            followPlayer = true;
        }
    }

    void Update () {
        if (!followPlayer)
        {
            return;
        }
        StartCoroutine(CheckForTarget());
    }

    IEnumerator CheckForTarget()
    {
        if (Vector3.Distance(target.position,transform.position) > distanceToFollowPlayer)
        {
            followPlayer = false;
            target = castle;
        }
        //set path to target
        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            navMeshAgent.SetPath(path);
        }
        else
        {
            Debug.Log("Enemy could not set path");
        }
        yield return new WaitForSeconds(1f);
    }
}
