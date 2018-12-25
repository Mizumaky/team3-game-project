using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody), typeof (Collider))]
public class FireballProjectile : MonoBehaviour {
  #region Variables
  public GameObject explosionPrefab;

  [Header ("Scriptable Parameters")]
  private float _damage;
  public float damage { get { return _damage; } }

  private Transform casterTransform;
  private LayerMask collisionMask;

  private float timeToLive;
  private float travelHeight;
  private bool isTraveling;

  private GameObject explosion;

  #endregion

  public void Set (float damage, Transform casterTransform, float travelHeight, float timeToLive, LayerMask collisionMask) {
    this._damage = damage;
    this.casterTransform = casterTransform;
    this.travelHeight = travelHeight;
    this.timeToLive = timeToLive;
    this.collisionMask = collisionMask;

    isTraveling = true;
    StartCoroutine (FollowTerrain ());
  }

  private IEnumerator FollowTerrain () {
    float updateInterval = 0.05f;
    WaitForSeconds ws = new WaitForSeconds (updateInterval);
    Rigidbody rigidbody = GetComponent<Rigidbody> ();

    Ray rayDown;
    RaycastHit hit;
    float rayLength = 10f;
    int layerMask = 1 << LayerMask.NameToLayer ("Ground");
    Vector3 newPos;

    transform.rotation = Quaternion.LookRotation (rigidbody.velocity);
    while (isTraveling && timeToLive > 0) {
      rayDown = new Ray (transform.position + Vector3.up * 5f, Vector3.down);
      if (Physics.Raycast (rayDown, out hit, rayLength, layerMask)) {
        newPos.x = transform.position.x;
        newPos.y = travelHeight + hit.point.y;
        newPos.z = transform.position.z;

        transform.position = newPos;
      }

      timeToLive -= updateInterval;
      yield return ws;
    }

    Disable ();
  }

  private void OnTriggerStay (Collider other) {
    if (isTraveling && UnityExtensions.ContainsLayer (collisionMask, other.gameObject.layer)) {
      if (other.gameObject.layer == LayerMask.NameToLayer ("Enemy")) {
        other.GetComponent<EnemyControls> ().AggroTo (casterTransform);
        other.GetComponent<EnemyStats> ().TakeDamage (damage);
      }

      isTraveling = false;

      explosion = Instantiate (explosionPrefab, transform.position, Quaternion.identity, null);
      explosion.transform.localScale = transform.localScale;

      HitEnemies ();

      Disable ();
    }
  }

  private void Disable () {
    Rigidbody rigidbody = GetComponent<Rigidbody> ();
    if (rigidbody != null) {
      rigidbody.useGravity = false;
      rigidbody.velocity = Vector3.zero;
    }

    GetComponent<ParticleSystem> ().Stop ();
    GetComponent<Collider> ().enabled = false;
    GetComponent<Animator> ().SetTrigger ("Fade");
  }

  private void Die () {
    if (explosion != null) {
      Destroy (explosion);
    }
    Destroy (gameObject, 1f);
  }

  private void HitEnemies () {
    int enemyLayer = 1 << LayerMask.NameToLayer ("Enemy");
    Collider[] hits = Physics.OverlapSphere (transform.position, transform.localScale.x, enemyLayer);

    foreach (Collider hit in hits) {
      hit.GetComponent<Stats> ().TakeDamage (damage);
      hit.GetComponent<EnemyControls> ().AggroTo (casterTransform);
    }
  }
}