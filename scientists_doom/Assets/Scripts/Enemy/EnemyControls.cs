using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControls : MonoBehaviour {

  private NavMeshAgent navMeshAgent;
  public Transform castle;
  private Transform target;
  private NavMeshPath path;
  [Range (5f, 10f)]
  public float distanceToFollowPlayer = 7f;
  private Animator animator;
  private float speed;
  private Vector3 lastPosition;
  private EnemyStats enemyStats;

  Coroutine activeFollowCoroutine;

  void Awake () {
    Init ();

    target = castle;
    SetPathToTarget (target);

    speed = 0;
  }

  private void Update () {
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
    if (activeFollowCoroutine == null) {
      Debug.Log ("Starting routine");
      activeFollowCoroutine = StartCoroutine (CheckForTargetAndFollow ());
    }
  }

  IEnumerator CheckForTargetAndFollow () {

    if (enemyStats.isAlive ()) {
      float distance = Vector3.Distance (target.position, transform.position);
      while (target != null && target.gameObject.activeSelf && distance < distanceToFollowPlayer) {
        distance = Vector3.Distance (target.position, transform.position);
        // If taraget close enough, attack and face it
        if (distance <= 1) {
          transform.rotation = CountLookRotation ();
          animator.SetTrigger ("attackTrigger");
        } else {
          SetPathToTarget (target);
          //Debug.Log ("Setting path");
        }
        yield return new WaitForSeconds (0.1f);
      }

      // Reset to castle
      target = castle;
      SetPathToTarget (target);
    }
  }

  private void SetPathToTarget (Transform target) {
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

  private Quaternion CountLookRotation () {
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
  }

}