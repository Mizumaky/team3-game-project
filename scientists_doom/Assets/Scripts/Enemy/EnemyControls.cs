using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControls : MonoBehaviour
{

  private NavMeshAgent navMeshAgent;
  public Transform castle;
  private Transform target;
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
    target = castle;
    StartCoroutine(AttackCastle());

    speed = 0;
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

  public void Aggro(Transform targetTransform)
  {
    target = targetTransform;

    // Interrupt previous follow
    if (isFollowing)
    {
      isFollowing = false;
    }

    // Start a new one
    StartCoroutine(AttackPlayer());
  }

  private IEnumerator AttackPlayer()
  {
    isFollowing = true;
    WaitForSeconds updatePeriod = new WaitForSeconds(0.1f);
    if (enemyStats.isAlive())
    {
      float distance = Vector3.Distance(target.position, transform.position);
      while (target != null && target.gameObject.activeSelf && distance < distanceToFollowPlayer && isFollowing)
      {
        distance = Vector3.Distance(target.position, transform.position);
        // If taraget close enough, attack and face it
        if (distance <= 1)
        {
          transform.rotation = CountLookRotation();
          animator.SetTrigger("attackTrigger");
        }
        else
        {
          SetPathToTarget(target);
        }
        yield return updatePeriod;
      }

      // Reset to castle
      target = castle;
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
      float distance = Vector3.Distance(target.position, transform.position);
      while (target != null && target.gameObject.activeSelf && isFollowing)
      {
        distance = Vector3.Distance(target.position, transform.position);
        // If target close enough, attack and face it
        if (distance <= 4)
        {
          transform.rotation = CountLookRotation();
          animator.SetTrigger("attackTrigger");
        }
        else
        {
          SetPathToTarget(target);
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
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z));
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

  public void SlowFor(float time)
  {
    if (!isSlowed)
    {
      StartCoroutine(SlowCountdown(time));
    }
  }

  private IEnumerator StunCountdown(float time)
  {
    isStunned = true;
    yield return new WaitForSeconds(time);
    isStunned = false;
    Aggro(target);
  }

  private IEnumerator SlowCountdown(float time)
  {
    float speed = navMeshAgent.speed;
    navMeshAgent.speed = speed / 3f;

    isSlowed = true;
    yield return new WaitForSeconds(time);
    isSlowed = false;

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