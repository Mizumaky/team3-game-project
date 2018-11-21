using System.Collections;
using UnityEngine;

public class StormCloud : MonoBehaviour {

  public Transform[] rayOrigins;
  public float hitRadius = 10f;
  public LayerMask hitMask;
  public float lightningDamage = 10f;
  private Transform casterTransform;

  public void SetCasterTransform (Transform transform) {
    casterTransform = transform;
  }

  private void Awake () {
    for (int i = 0; i < rayOrigins.Length; i++) {
      StartCoroutine (HitPeriodicly (i));
    }
  }

  private IEnumerator HitPeriodicly (int index) {
    WaitForSeconds hitPeriod = new WaitForSeconds (rayOrigins[index].GetComponent<ParticleSystem> ().main.duration);
    while (true) {
      Hit (index);
      yield return hitPeriod;
    }
  }

  private void Hit (int rayOriginIndex) {
    rayOrigins[rayOriginIndex].GetComponent<ParticleSystem> ().Play ();
    // Cast ray down from lighting origin
    Ray rayDown = new Ray (rayOrigins[rayOriginIndex].position, Vector3.down);
    int groundLayer = 1 << LayerMask.NameToLayer ("Ground");

    // Create overlap spthere at the point where the ray hits the ground
    RaycastHit rayHit;
    Collider[] sphereHit;
    if (Physics.Raycast (rayDown, out rayHit, 20f * transform.localScale.x, groundLayer)) {
      sphereHit = Physics.OverlapSphere (rayOrigins[rayOriginIndex].position, hitRadius * transform.localScale.x, hitMask);
    } else {
      sphereHit = new Collider[0];
    }

    // Layers
    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    int explosiveLayer = LayerMask.NameToLayer ("Explosive");

    foreach (Collider hit in sphereHit) {
      if (hit.gameObject.layer == enemyLayer) {
        if (casterTransform == null) {
          //Debug.LogWarning ("Storm Cloud has no caster transform!");
        } else {
          hit.GetComponent<EnemyControls> ().Aggro (casterTransform);
        }
        hit.GetComponent<EnemyStats> ().TakeDamage (lightningDamage * transform.localScale.x);
      }

      if (hit.gameObject.layer == explosiveLayer) {
        hit.GetComponent<Explosive> ().Explode (casterTransform);
      }
    }
  }

}