using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControls : MonoBehaviour {

  protected NavMeshAgent navMeshAgent;
  public Transform castle;
  protected Transform target;
  protected NavMeshPath path;
  [Range (5f, 20f)]
  public float distanceToFollowPlayer = 7f;
  public float playerAttackReach;
  protected float castleAttackReach;
  protected Animator animator;
  protected float speed;
  protected Vector3 lastPosition;
  protected EnemyStats enemyStats;
  public float targetPositionUpdatePeriod;

  protected Coroutine activeFollowCoroutine;

  void Awake () {
    Init ();
  }

  void Start () {
    castleAttackReach = playerAttackReach +  2; //castle transform center offset
    target = castle;
    activeFollowCoroutine = StartCoroutine (AttackCastle ());

    speed = 0;
  }

  protected void Update () {
    if (enemyStats.isAlive ()) {
      if (animator != null) {
        speed = Mathf.Lerp (speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f) / navMeshAgent.speed;
        lastPosition = transform.position;

        animator.SetFloat ("speedParam", speed);
      }
    }
  }

  private void Init () {
    navMeshAgent = GetComponent<NavMeshAgent> ();
    path = new NavMeshPath ();
    animator = GetComponent<Animator> ();
    enemyStats = GetComponent<EnemyStats> ();
  }

  public void Aggro (Transform targetTransform) {
    Debug.Log ("Aggroing to " + targetTransform.name);
    target = targetTransform;

    if (activeFollowCoroutine != null) {
      StopCoroutine (activeFollowCoroutine);
    }
    //Debug.Log ("Starting AttackPlayer coroutine");
    activeFollowCoroutine = StartCoroutine (AttackPlayer ());
  }

  protected virtual IEnumerator AttackPlayer () {
    WaitForSeconds updatePeriod = new WaitForSeconds (0.1f);
    if (enemyStats.isAlive ()) {
      float distance = Vector3.Distance (target.position, transform.position);
      while (target != null && target.gameObject.activeSelf && distance < distanceToFollowPlayer) {
        distance = Vector3.Distance (target.position, transform.position);
        // If taraget close enough, attack and face it
        if (distance <= playerAttackReach) {
          navMeshAgent.isStopped = true;
          transform.rotation = CountLookRotation ();
          animator.SetTrigger ("attackTrigger");
        } else {
          navMeshAgent.isStopped = false;
          SetPathToTarget (target);
          //Debug.Log ("Setting path");
        }
        yield return updatePeriod;
      }

      // Reset to castle
      target = castle;
      activeFollowCoroutine = StartCoroutine (AttackCastle ());
    }
  }

  protected virtual IEnumerator AttackCastle () {
    WaitForSeconds updatePeriod = new WaitForSeconds (0.1f);
    if (enemyStats.isAlive ()) {
      float distance = Vector3.Distance (target.position, transform.position);
      while (target != null && target.gameObject.activeSelf) {
        distance = Vector3.Distance (target.position, transform.position);
        // If target close enough, attack and face it
        if (distance <= castleAttackReach) {
          navMeshAgent.isStopped = true;
          transform.rotation = CountLookRotation ();
          animator.SetTrigger ("attackTrigger");
        } else {
          navMeshAgent.isStopped = false;
          SetPathToTarget (target);
          //Debug.Log ("Setting path");
        }
        yield return updatePeriod;
      }
    }
  }

  protected void SetPathToTarget (Transform target) {
    if (target != null && gameObject != null) {

      NavMeshHit hitNavmesh;
      NavMesh.SamplePosition (target.position, out hitNavmesh, 50f, 5);

      if (navMeshAgent.enabled && NavMesh.CalculatePath (transform.position, hitNavmesh.position, NavMesh.AllAreas, path)) {
        navMeshAgent.SetPath (path);
      } else {
        Debug.Log ("Enemy could not set path");
      }
    }
  }

  protected Quaternion CountLookRotation () {
    Quaternion lookRotation = Quaternion.LookRotation (new Vector3 (target.position.x - transform.position.x, 0, target.position.z - transform.position.z));
    return Quaternion.Slerp (transform.rotation, lookRotation, 0.3f);
  }

  public void StunFor (float time) {
    DisableMovement ();
    StartCoroutine (StunCountdown (time));
  }

  public IEnumerator StunCountdown (float time) {
    yield return new WaitForSeconds (time);
    Aggro (target);
  }

  public void DisableMovement () {
    StopAllCoroutines ();
    activeFollowCoroutine = null;
    navMeshAgent.ResetPath ();
  }

  public void DisableCollision () {
    GetComponent<CapsuleCollider> ().enabled = false;
    GetComponent<Rigidbody> ().isKinematic = true;
  }
}