using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControls : MonoBehaviour
{

  private NavMeshAgent navMeshAgent;
  public Transform castleTransform;
  private Transform targetTransform;
  private NavMeshPath path;
  [Range(5f, 10f)]
  public float distanceToFollowPlayer = 7f;
  private Animator animator;
  private float speed;
  private Vector3 lastPosition;
  private EnemyStats enemyStats;
  public float targetPositionUpdatePeriod;

  public bool isFollowing;
  public bool isStunned;
  public bool isSlowed;

  void Awake()
  {
    Init();
  }

  void Start()
  {
    targetTransform = castleTransform;
    StartCoroutine(AttackCastle());
  }

  private void Update()
  {
    if (enemyStats.isAlive())
    {
      if (animator != null)
      {
        speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f) / navMeshAgent.speed;
        lastPosition = transform.position;

        animator.SetFloat("speedParam", speed);
      }
    }
  }

  private void Init()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    path = new NavMeshPath();
    animator = GetComponent<Animator>();
    enemyStats = GetComponent<EnemyStats>();
  }

  /// <summary>
  /// Sets the enemy's target and makes it follow 
  /// </summary>
  /// <param name="targetTransform"></param>
  public void AggroTo(Transform targetTransform)
  {
    if (this.targetTransform != targetTransform)
    {
      this.targetTransform = targetTransform;
    }

    if (!isFollowing)
    {
      StartCoroutine(AttackPlayer());
    }
  }

  private IEnumerator AttackPlayer()
  {
    isFollowing = true;
    WaitForSeconds updatePeriod = new WaitForSeconds(0.1f);
    if (enemyStats.isAlive())
    {
      float distance = Vector3.Distance(targetTransform.position, transform.position);
      while (targetTransform != null && targetTransform.gameObject.activeSelf && distance < distanceToFollowPlayer && isFollowing)
      {
        distance = Vector3.Distance(targetTransform.position, transform.position);
        // If taraget close enough, attack and face it
        if (distance <= 1)
        {
          transform.rotation = CountLookRotation();
          animator.SetTrigger("attackTrigger");
        }
        else
        {
          SetPathToTarget(targetTransform);
        }
        yield return updatePeriod;
      }

      // Reset to castle
      targetTransform = castleTransform;
      StartCoroutine(AttackCastle());
    }
    isFollowing = false;
  }

  private IEnumerator AttackCastle()
  {
    isFollowing = true;
    WaitForSeconds updatePeriod = new WaitForSeconds(0.1f);
    if (enemyStats.isAlive())
    {
      float distance = Vector3.Distance(targetTransform.position, transform.position);
      while (targetTransform != null && targetTransform.gameObject.activeSelf && isFollowing)
      {
        distance = Vector3.Distance(targetTransform.position, transform.position);
        // If target close enough, attack and face it
        if (distance <= 4)
        {
          transform.rotation = CountLookRotation();
          animator.SetTrigger("attackTrigger");
        }
        else
        {
          SetPathToTarget(targetTransform);
        }
        yield return updatePeriod;
      }
    }
    isFollowing = false;
  }

  private void SetPathToTarget(Transform target)
  {
    if (target != null && gameObject != null)
    {

      NavMeshHit hitNavmesh;
      NavMesh.SamplePosition(target.position, out hitNavmesh, 50f, 5);

      if (navMeshAgent.enabled && NavMesh.CalculatePath(transform.position, hitNavmesh.position, NavMesh.AllAreas, path))
      {
        navMeshAgent.SetPath(path);
      }
      else
      {
        Debug.Log("Enemy could not set path");
      }
    }
  }

  private Quaternion CountLookRotation()
  {
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z));
    return Quaternion.Slerp(transform.rotation, lookRotation, 0.3f);
  }

  // FIXME: Could be optimized maybe
  public void StunFor(float time)
  {
    if (!isStunned)
    {
      DisableMovement();
      StartCoroutine(StunCountdown(time));
    }
  }

  public void Slow()
  {
    StartCoroutine(SlowRoutine());
  }

  private IEnumerator StunCountdown(float time)
  {
    isStunned = true;
    yield return new WaitForSeconds(time);
    isStunned = false;
    AggroTo(targetTransform);
  }

  private IEnumerator SlowRoutine()
  {
    float speed = navMeshAgent.speed;
    navMeshAgent.speed = speed / 3f;

    isSlowed = true;
    while (isSlowed)
    {
      yield return null;
    }

    navMeshAgent.speed = speed;
  }

  public void DisableMovement()
  {
    isFollowing = false;
    navMeshAgent.ResetPath();
  }

  public void DisableCollision()
  {
    GetComponent<CapsuleCollider>().enabled = false;
    GetComponent<Rigidbody>().isKinematic = true;
  }

}