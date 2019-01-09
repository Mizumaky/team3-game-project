using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyControls : EnemyControls
{
  public GameObject projectile;
  public GameObject attackSpawnPoint;
  private int mask;
  private int direction;
  private float raycastLen;
  private int raycastMissCnt = 0; //if more than 5 miss than switch target to caslte

  protected override void Awake()
  {
    base.Awake();
    //count LayerMask for Raycast
    mask = (1 << LayerMask.NameToLayer("Player"))
      | (1 << LayerMask.NameToLayer("Castle"))
      | (1 << LayerMask.NameToLayer("Enemy"))
      | (1 << LayerMask.NameToLayer("Obstacle")); //in case that unity changes numbers of leyers
    direction = Random.Range(0, 2);
    raycastLen = attackCastleDistance > attackPlayerDistance ? attackCastleDistance : attackPlayerDistance;
  }

  protected override void Attack()
  {
    if (FreeViewToTarget())
    {
      raycastMissCnt = 0;
      // FIXME: Remove or opt the rotation?
      Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z));
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.4f);
      navMeshAgent.isStopped = true;
      // FIXME: Animation should start attack...
      animator.SetTrigger("attackTrigger");
    }
    else
    {
      StepAside();
    }
  }

  private void Fire() //to the animation event
  {
    GameObject pr = Instantiate(projectile, attackSpawnPoint.transform.position, attackSpawnPoint.transform.rotation);
    pr.GetComponent<RangedEnemyProjectiles>().SetDamage(gameObject.GetComponent<Stats>().GetAttackDamage());
    pr.GetComponent<RangedEnemyProjectiles>().Fire(targetTransform.position);
    Destroy(pr, 2);
  }

  private bool FreeViewToTarget()
  {
    RaycastHit hit;
    bool ret;
    if (Physics.Raycast(attackSpawnPoint.transform.position, (targetTransform.position - transform.position).normalized, out hit, raycastLen, mask))
    {
      ret = (hit.transform.position == targetTransform.position) ? true : false;
    }
    else
    {
      Debug.LogWarning("Enemy FreeViewToTarget() raycast miss, " + targetTransform.name + targetTransform.position);
      if(targetTransform.name != "Eagle(Clone)") {
        raycastMissCnt++;
      }
      if (raycastMissCnt >= 5) {
        targetTransform = castleTransform;
      }
      ret = false;
    }
    return ret;
  }

  private void StepAside()
  {
    Vector3 dir = (targetTransform.position - transform.position).normalized;
    Vector3 cross = (direction == 0) ? new Vector3(-dir.z, 0, dir.x) : new Vector3(dir.z, 0, -dir.x);
    navMeshAgent.isStopped = false;
    SetPathToTargetPosition(transform.position + 2 * cross.normalized);
  }
}
