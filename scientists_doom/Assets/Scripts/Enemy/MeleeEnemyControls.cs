using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyControls : EnemyControls {

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
          animator.SetTrigger("attackTrigger");
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
          navMeshAgent.isStopped = true;
          transform.rotation = CountLookRotation();
          animator.SetTrigger("attackTrigger");
        } else {
          navMeshAgent.isStopped = false;
          SetPathToTarget(target);
          //Debug.Log ("Setting path");
        }
        yield return updatePeriod;
      }
    }
  }
}
