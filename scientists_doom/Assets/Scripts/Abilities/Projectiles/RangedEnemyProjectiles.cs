using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectiles : MonoBehaviour
{
  private float damage;
  private Vector3 targetPos;
  public float YAxisOffset; // siege machine = 15, support = 2
  public float velocity; // siege machine = 18, support = 15
  public float timeToLiveAfterHit;

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

  public void Fire(Vector3 pos) {
    targetPos = pos;
    Vector3 vel = targetPos - transform.position;
    vel = new Vector3(vel.x, vel.y + YAxisOffset, vel.z);
    GetComponent<Rigidbody>().velocity = vel.normalized * velocity;
    GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
  }

  private void Die() {
    //particle effect
    Destroy(gameObject, timeToLiveAfterHit);
  }
}
