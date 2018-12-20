using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyControls : EnemyControls {

  public GameObject projectile;
  public GameObject attackSpawnPoint;
  private int mask;
  int direction;

  protected override void Awake() {
    base.Awake();
    //count LayerMask for Raycast
    mask = (1 << LayerMask.NameToLayer("Player")) 
      | (1 << LayerMask.NameToLayer("Castle")) 
      | (1 << LayerMask.NameToLayer("Enemy")) 
      | (1 << LayerMask.NameToLayer("Obstacle")); //in case that unity changes numbers of leyers
    direction = Random.Range(0, 2);
  }

  protected override void Attack() {
    if (FreeViewToTarget()) {
      // FIXME: Remove or opt the rotation?
      Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z));
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.4f);
      navMeshAgent.isStopped = true;
      // FIXME: Animation should start attack...
      ShootProjectile();
    } else {
      StepAside();
    }
  }

  private void ShootProjectile() {
    GameObject pr = Instantiate(projectile);
    pr.transform.position = attackSpawnPoint.transform.position;
    pr.transform.rotation = attackSpawnPoint.transform.rotation;
    pr.GetComponent<Rigidbody>().velocity = targetTransform.position - transform.position;
    Destroy(pr, 2);
  }

  private bool FreeViewToTarget() {
    RaycastHit hit;
    bool ret;
    if (Physics.Raycast(transform.position, (targetTransform.position - transform.position).normalized, out hit, 20, mask)) {
      ret = (hit.transform.position == targetTransform.position) ? true : false;
    } else {
      Debug.LogWarning("Enemy FreeViewToTarget() raycast miss");
      ret = false;
    }
    return ret;
  }

  private void StepAside() {
    Vector3 dir = (targetTransform.position - transform.position).normalized;
    Vector3 cross = (direction == 0) ? new Vector3(-dir.z, 0, dir.x) : new Vector3(dir.z, 0, -dir.x);
    navMeshAgent.isStopped = false;
    SetPathToTargetPosition(transform.position + 2 * cross.normalized);    
  }
}
