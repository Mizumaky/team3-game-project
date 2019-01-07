using UnityEngine;
using UnityEngine.AI;

public class CustomMovement : MonoBehaviour {
  public Transform targetPointTransform;
  public bool walk;
  private bool isWalking = false;
  private NavMeshAgent agent;
  private Animator animator;
  private float speed;
  private Vector3 lastPosition;

  private void Awake() {
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    lastPosition = transform.position;
  }

  private void Update() {
    if (!isWalking && walk) {
      isWalking = true;
      NavMeshPath path = new NavMeshPath();

      if (NavMesh.CalculatePath(transform.position, targetPointTransform.position, NavMesh.AllAreas, path))
      {
        agent.SetPath(path);
      } else {
        Debug.LogWarning("No path to target!");
      }
    } else if (isWalking && !walk) {
      isWalking = false;
      agent.ResetPath();
      if (animator != null)
      {
        animator.SetFloat("speedParam", 0f);
      }
    }

    if (animator != null)
    {
      speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.75f) / 3.5f;
      lastPosition = transform.position;

      animator.SetFloat("speedParam", speed);
    }
  }
}