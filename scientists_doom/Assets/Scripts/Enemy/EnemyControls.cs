﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EnemyStats))]
public class EnemyControls : MonoBehaviour
{
  public enum State { Normal, Stunned, Taunted }

  [Header("Refs")]
  public Transform castleTransform;

  [Header("Parameters")]
  public State state = State.Normal;
  [Range(5f, 15f)]
  public float aggroDistance = 10f;
  public float attackPlayerDistance = 1f;
  public float attackCastleDistance = 4f;
  public float behaviourUpdatePeriod = 0.1f;

  [Header("Drop After Death")]
  public GameObject lootPrefab;

  private NavMeshAgent navMeshAgent;
  private Animator animator;
  private EnemyStats stats;

  private Transform targetTransform;
  private float distanceToTarget;

  private float speed;
  private int slowedTimes = 0;
  private int stunnedTimes = 0;

  private void Awake()
  {
    Init();
  }

  private void Start()
  {
    StartCoroutine(UpdateBehaviourRoutine());
    StartCoroutine(UpdateAnimatorRoutine());
  }

  private void Init()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    speed = navMeshAgent.speed;
    animator = GetComponent<Animator>();
    stats = GetComponent<EnemyStats>();
  }

  private IEnumerator UpdateAnimatorRoutine()
  {
    float speedParam = 0;
    Vector3 lastPosition = transform.position;

    while (stats.isAlive)
    {
      if (animator.isActiveAndEnabled)
      {
        speedParam = Mathf.Lerp(speedParam, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f) / navMeshAgent.speed;
        lastPosition = transform.position;

        animator.SetFloat("speedParam", speedParam);
      }
      yield return null;
    }
  }

  private IEnumerator UpdateBehaviourRoutine()
  {
    WaitForSeconds waitForUpdate = new WaitForSeconds(behaviourUpdatePeriod);

    while (stats.isAlive)
    {
      if (stunnedTimes <= 0)
      {
        if (targetTransform == null)
        {
          targetTransform = castleTransform;
        }

        distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        float attackDistance = (targetTransform == castleTransform ? attackCastleDistance : attackPlayerDistance);

        // ATTACK
        if (distanceToTarget < attackDistance)
        {
          // FIXME: Remove or opt the rotation?
          Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z));
          transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.3f);

          navMeshAgent.isStopped = true;
          Attack();
        }
        // MOVE TOWARDS TARGET
        else
        {
          navMeshAgent.isStopped = false;
          // TARGET TOO FAR, RETARGET TO CASTLE
          if (distanceToTarget > aggroDistance)
          {
            AggroTo(castleTransform);
          }
          SetPathToTargetPosition(targetTransform.position);
        }
      }
      yield return waitForUpdate;
    }

    yield return null;
  }

  protected virtual void Attack()
  {
    animator.SetTrigger("attackTrigger");
  }

  /// <summary>
  /// Sets the enemy's target and makes it follow 
  /// </summary>
  /// <param name="targetTransform"></param>
  public void AggroTo(Transform targetTransform)
  {
    this.targetTransform = targetTransform;
  }

  private void SetPathToTargetPosition(Vector3 targetPosition)
  {
    NavMeshHit hit;
    NavMeshPath path = new NavMeshPath();
    // FIXME: Shorten the distance for opt maybe?
    float distanceFromSource = 50f;

    bool nearestPointFound = NavMesh.SamplePosition(targetPosition, out hit, distanceFromSource, 5);
    bool pathFound = nearestPointFound && NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path);

    if (navMeshAgent.enabled && pathFound)
    {
      navMeshAgent.SetPath(path);
    }
    else
    {
      Debug.LogWarning("Navmesh path not found!");
    }
  }

  public void Disable()
  {
    StopAllCoroutines();

    Destroy(GetComponent<Rigidbody>());
    GetComponent<Collider>().enabled = false;
    navMeshAgent.enabled = false;

    animator.SetTrigger("dieTrigger");

    SpawnLoot();
    Destroy(gameObject, 3f);
  }

  public void Stun()
  {
    state = State.Stunned;
    stunnedTimes++;
    navMeshAgent.isStopped = true;
  }

  public void Slow()
  {
    slowedTimes++;
    navMeshAgent.speed = navMeshAgent.speed / 3;
  }

  public void RemoveSlow()
  {
    slowedTimes--;
    navMeshAgent.speed = navMeshAgent.speed * 3;
  }

  private void SpawnLoot()
  {
    float OffsetY = 0.5f;
    Vector3 lootPosition = new Vector3(transform.position.x, transform.position.y + OffsetY, transform.position.z);
    GameObject loot = Instantiate(lootPrefab, lootPosition, transform.rotation * Quaternion.Euler(-90, 0, 0));
  }
}