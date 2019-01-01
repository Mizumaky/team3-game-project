using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class SiegeMachineController : EnemyControls {

  public GameObject projectile;
  public GameObject projectilePlaceHolder;
  public Transform projectileSpawnPoint;

  protected override IEnumerator UpdateBehaviourRoutine() {
    WaitForSeconds waitForUpdate = new WaitForSeconds(behaviourUpdatePeriod);
    float attackDistance = attackCastleDistance;
    while (stats.isAlive) {
      if (stunnedTimes <= 0) {
        if (targetTransform == null) {
          targetTransform = castleTransform;
        }
        distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        
        if (distanceToTarget < attackDistance) {
          Attack();
        }
        // MOVE TOWARDS TARGET
        else {
          if (navMeshAgent.isOnNavMesh)
            navMeshAgent.isStopped = false;
          SetPathToTargetPosition(targetTransform.position);
        }
      }
      yield return waitForUpdate;
    }
    yield return null;
  }

  protected override void Attack() {
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetTransform.position.x - transform.position.x, 0, targetTransform.position.z - transform.position.z));
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.4f);
    navMeshAgent.isStopped = true;
    animator.SetTrigger("shootTrigger");
  }

  public void Shoot() {
    GameObject pr = (GameObject)Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    projectilePlaceHolder.SetActive(false);
    pr.GetComponent<RangedEnemyProjectiles>().SetDamage(gameObject.GetComponent<Stats>().GetAttackDamage());
    pr.GetComponent<RangedEnemyProjectiles>().Fire(targetTransform.position);
    Destroy(pr, 2);
  }

  public override void AggroTo(Transform targetTransform) {
    //do nothing... Siege machine attack only castle
  }

  public void EnablePlaceholder() {
    projectilePlaceHolder.SetActive(true);
  }

 }
