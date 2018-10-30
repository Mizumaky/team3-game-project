using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProjectile : CharacterAbility {

  [Header ("Projectile")]
  public float velocity = 20f;
  public float lifeTime = 2f;
  public float flightHeightFromTerrain = 0.5f;
  private Light thisLight;
  private float initialLightIntensity;
  private bool collisionEnabled = false;
  [Space]

  [Header ("Impact Effect")]
  public GameObject impactEffect;
  public float impactRadius = 0.5f;
  CustomUpdateTimer impactEndTimer;

  public void SetCollisionEnabled (bool value) {
    collisionEnabled = value;
  }

  private void Awake () {
    thisLight = GetComponent<Light> ();
    initialLightIntensity = thisLight.intensity;

  }

  private void Update () {
    // Destroy GO after impact timer has ended
    if (impactEndTimer != null) {
      if (impactEndTimer.isLastTick (Time.deltaTime)) {
        Destroy (gameObject);
      } else {
        thisLight.intensity = initialLightIntensity * impactEndTimer.GetTimeLeftPercent ();
      }
    }
  }

  public void Release () {
    StartCoroutine (UpdateHeight ());
    collisionEnabled = true;
  }

  private IEnumerator UpdateHeight () {
    int groundLayerIndex = LayerMask.NameToLayer ("Ground");

    while (true) {
      Ray rayDown = new Ray (transform.position, Vector3.down);
      RaycastHit hit;
      if (Physics.Raycast (rayDown, out hit, 2f)) {
        if (hit.collider.gameObject.layer == groundLayerIndex) {
          transform.position = new Vector3 (transform.position.x, flightHeightFromTerrain + hit.point.y, transform.position.z);
        }
      }
      yield return null;
    }

  }

  private void OnTriggerEnter (Collider other) {
    if (collisionEnabled) {
      // Enemy
      if (other.gameObject.layer == LayerMask.NameToLayer ("Enemy")) {
        Explode ();
      }
    }
  }

  private void Explode () {
    collisionEnabled = false;
    CheckHitsAndDealDamage ();

    Destroy (GetComponent<Rigidbody> ());
    GetComponent<ParticleSystem> ().Stop ();

    GameObject explosionInstance = Instantiate (impactEffect, transform.position, transform.rotation, transform);
    explosionInstance.transform.localScale = transform.localScale;

    impactEndTimer = new CustomUpdateTimer (0.9f * impactEffect.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void CheckHitsAndDealDamage () {
    Collider[] hits = Physics.OverlapSphere (transform.position, impactRadius);

    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    foreach (Collider hit in hits) {
      if (hit.gameObject.layer == enemyLayer) {
        hit.GetComponent<EnemyStats> ().TakeDamage (damage);
      }
    }
  }

  private void OnDrawGizmosSelected () {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere (transform.position, impactRadius);
  }
}