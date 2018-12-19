using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyControls : EnemyControls {

  public GameObject projectile;
  public GameObject attackSpawnPoint;

  protected override IEnumerator AttackPlayer() {
    WaitForSeconds updatePeriod = new WaitForSeconds(0.1f);
    if (enemyStats.isAlive()) {
      float distance = Vector3.Distance(target.position, transform.position);
      while (target != null && target.gameObject.activeSelf && distance < distanceToFollowPlayer) {
        distance = Vector3.Distance(target.position, transform.position);
        // If taraget close enough, attack and face it
        if (distance <= playerAttackReach) {
          navMeshAgent.isStopped = true;
          transform.rotation = CountLookRotation();
          //animator.SetTrigger("attackTrigger");
          ShootProjectile(); //TODO: do shooting by animation
        } else {
          navMeshAgent.isStopped = false;
          SetPathToTarget(target);
          //Debug.Log ("Setting path");
        }
        yield return updatePeriod;
      }

      // Reset to castle
      target = castle;
      activeFollowCoroutine = StartCoroutine(AttackCastle());
    }
  }

  protected override IEnumerator AttackCastle() {
    WaitForSeconds updatePeriod = new WaitForSeconds(0.1f);
    if (enemyStats.isAlive()) {
      float distance = Vector3.Distance(target.position, transform.position);
      while (target != null && target.gameObject.activeSelf) {
        distance = Vector3.Distance(target.position, transform.position);
        // If target close enough, attack and face it
        if (distance <= castleAttackReach) {
          //TODO: checkViewToTarget if false make a few steps and than check again
          navMeshAgent.isStopped = true;
          transform.rotation = CountLookRotation();
          //animator.SetTrigger("attackTrigger");
          ShootProjectile(); //TODO: do shooting by animation
        } else {
          navMeshAgent.isStopped = false;
          SetPathToTarget(target);
          //Debug.Log ("Setting path");
        }
        yield return updatePeriod;
      }
    }
  }

  private void ShootProjectile() {
    GameObject pr = Instantiate(projectile);
    pr.transform.position = attackSpawnPoint.transform.position;
    pr.transform.rotation = attackSpawnPoint.transform.rotation;
    pr.GetComponent<Rigidbody>().velocity = target.position - transform.position;
    Destroy(pr, 2);
  }

  private bool checkFreeViewToTarget() { //Raycast to target

    return true;
  }
}
