using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyControls : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    public Transform castle;
    private Transform target;
    private NavMeshPath path;
    [Range(5f, 10f)]
    public float distanceToFollowPlayer = 7f;
    private Animator animator;
    private float speed;
    private Vector3 lastPosition;
    public bool triggered;
    private EnemyStats enemyStats;
    private BoxCollider weponCollider;

    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        target = castle;
        SetPathToTarget(target);
        animator = GetComponent<Animator>();
        speed = 0;
        triggered = false;
        enemyStats = GetComponent<EnemyStats>();
        weponCollider = GetComponentInChildren<BoxCollider>();
    }

    private void Update() {
        if (enemyStats.enemyAlive) {
            if (animator != null) {
                speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f) / navMeshAgent.speed;
                lastPosition = transform.position;

                animator.SetFloat("speedParam", speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other != null && other.gameObject.layer == 11 && other.gameObject.tag == "Weapon") // 11. Player layer
        {
            if (!triggered)
                StartCoroutine(CheckForTarget());

            triggered = true;
            float dmg;
            if (other.gameObject.GetComponentInParent<PlayerAttacksBarbarian>()) {
                target = other.GetComponentInParent<PlayerControls>().transform;
            } else {
                target = other.GetComponentInChildren<CharacterProjectile>().casterTransform;
                dmg = other.GetComponentInChildren<CharacterProjectile>().damage;
                enemyStats.TakeDamage(dmg);
            }
            
        }

    }

    IEnumerator CheckForTarget() {

        if (enemyStats.enemyAlive) {
            float distance = Vector3.Distance(target.position, transform.position);
            while (target != null && target.gameObject.activeSelf && distance < distanceToFollowPlayer) {

                distance = Vector3.Distance(target.position, transform.position);
                if (distance <= 1) {
                    transform.rotation = CountLookRotation();
                    animator.SetTrigger("attackTrigger");
                } else {
                    SetPathToTarget(target);
                }
                yield return new WaitForSeconds(0.1f);
            }
            target = castle;
            triggered = false;
            SetPathToTarget(target);
        }
    }

    private void SetPathToTarget(Transform target) {
        if (target != null && gameObject != null) {

            if (navMeshAgent.enabled && NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path)) {
                navMeshAgent.SetPath(path);
            } else {
                Debug.Log("Enemy could not set path");
            }
        }
    }

    private Quaternion CountLookRotation() {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));
        return Quaternion.Slerp(transform.rotation, lookRotation, 0.3f);
    }

    public void DisableMovement() {
        StopAllCoroutines();
        navMeshAgent.ResetPath();
    }

    public void DisableCollision() {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public void AttackStart() {
        weponCollider.enabled = true;
    }

    public void AttackEnd() {
        weponCollider.enabled = false;
    }

}
