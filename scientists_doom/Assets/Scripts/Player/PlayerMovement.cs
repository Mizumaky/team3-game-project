using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
  private NavMeshAgent navMeshAgent;
  public float distance = 50f;
  public float rotationSpeed = 10f;

  protected Animator animator;
  private float speed;
  private Vector3 lastPosition;

  private void Awake()
  {
    navMeshAgent = GetComponent<NavMeshAgent>();
    animator = GetComponentInChildren<Animator>();
  }

  void Start()
  {
    speed = 0;
  }

  //TODO add if state not moving tak at se to nejak resetne do default
  public void StopMoving()
  {
    navMeshAgent.isStopped = true;
    navMeshAgent.ResetPath();
    navMeshAgent.enabled = false;
    if (animator != null)
    {
      animator.SetFloat("speedParam", 0f);

    }
  }
  public void StartMoving()
  {
    navMeshAgent.enabled = true;
    navMeshAgent.isStopped = false;
  }

  public void Move()
  {
    Vector3 groundPositionVector = GetGroundPosAtMouse();
    if (groundPositionVector == Vector3.zero)
    {
      groundPositionVector = transform.position;
    }

    Vector3 direction = (groundPositionVector - transform.position).normalized;
    if (direction != new Vector3(0, 0, 0))
    {
      Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    if (Input.GetMouseButton(0))
    {
      if (!IsPointerOverRaycastBlockingUIObject())
      {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(transform.position, groundPositionVector, NavMesh.AllAreas, path))
        {
          navMeshAgent.SetPath(path);
        }
        else
        {
          Debug.Log("Could not set path");
        }
      }
    }
    else
    {
      navMeshAgent.ResetPath();
    }

    if (animator != null)
    {

      speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
      lastPosition = transform.position;

      animator.SetFloat("speedParam", speed);

    }
  }

  public static Vector3 GetGroundPosAtMouse()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

    int groundLayer = 1 << LayerMask.NameToLayer("Ground");
    if (Physics.Raycast(ray, out hit, 150f, groundLayer))
    {
      NavMeshHit hitNavmesh;
      NavMesh.SamplePosition(hit.point, out hitNavmesh, 50f, 5);
      return hitNavmesh.position;
    }

    return Vector3.zero;
  }

  private bool IsPointerOverRaycastBlockingUIObject()
  {
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

    int blockRaycastLayer = LayerMask.NameToLayer("UIBlockRaycast");
    foreach (RaycastResult result in results)
    {
      if (result.gameObject.layer == blockRaycastLayer)
      {
        return true;
      }
    }
    return false;
  }
}