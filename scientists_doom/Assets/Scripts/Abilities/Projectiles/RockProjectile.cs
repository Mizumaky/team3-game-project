using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{
  private float damage = 5;
  private float timeToLive = 4f;
  private Vector3 targetPos;

  void OnCollisionEnter(Collision collision) {
    int layer = collision.collider.gameObject.layer;
    if (layer == LayerMask.NameToLayer("Castle") || layer == LayerMask.NameToLayer("Player")) {
      collision.collider.gameObject.GetComponent<Stats>().TakeDamage(damage);
      EventManager.TriggerEvent("updateHUD");
      Die();
    }
  }

  public void SetDamage(float dmg) {
    damage = dmg;
  }

  public void SetTargetPos(Vector3 pos) {
    targetPos = pos;
    SetFlightVelocity();
  }

  private void SetFlightVelocity() {
    gameObject.GetComponent<Rigidbody>().velocity = targetPos - transform.position;
  }

  private void Die() {
    //particle effect
    Destroy(gameObject);
  }
}
