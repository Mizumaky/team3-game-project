#define DEBUG

using System.Collections;
using UnityEngine;

public class CharacterAbilityInstance : MonoBehaviour {

  [Header ("Carry")]
  [SerializeField] protected Transform casterTransform;
  [SerializeField] protected float damage;
  [Space]

  [Header ("Object Settings")]
  [SerializeField] protected new Rigidbody rigidbody;
  [SerializeField] protected LayerMask mask;
  [SerializeField] protected float velocity = 0f;
  [SerializeField] protected float lifeTime = 2f;
  [SerializeField] protected float hightFromTerrain = 0.5f;
  [Space]
  [SerializeField] protected new Light light;
  [SerializeField] protected new Collider collider;
  [SerializeField] protected float initialLightIntensity;
  [Space]
  private bool collisionEnabled = false;
  [Space]

  [Header ("Impact Effect")]
  public GameObject impactEffect;
  public float impactRadius = 0.5f;
  CustomUpdateTimer impactEndTimer;

  #region GettersSetters
  public float GetVelocity () {
    return velocity;
  }

  public float GetImpactRadius () {
    return impactRadius;
  }
  #endregion

  private void Awake () {
    collider = GetComponent<Collider> ();
    collider.enabled = false;
    if (light != null) {
      initialLightIntensity = light.intensity;
    }
  }

  private void Update () {
    // Destroy GO after impact timer has ended
    if (impactEndTimer != null) {
      if (impactEndTimer.isLastTick (Time.deltaTime)) {
        Destroy (gameObject);
      } else {
        if (light != null) {
          light.intensity = initialLightIntensity * impactEndTimer.GetTimeLeftPercent ();
        }
      }
    }
  }

  public virtual void SetAndRelease (Transform casterTransform, Vector3 velocity, float damage, float impactRadius) {
    this.casterTransform = casterTransform;
    transform.parent = null;
    rigidbody.velocity = velocity;
    this.damage = damage;
    this.impactRadius = impactRadius;

    Release ();
  }

  public void Drop () {
#if DEBUG
    Debug.Log ("Dropping");
#endif
    collisionEnabled = false;
    collider.enabled = true;

    int groundLayerIndex = LayerMask.NameToLayer ("Ground");

    Ray rayDown = new Ray (transform.position, Vector3.down);
    RaycastHit hit;
    if (Physics.Raycast (rayDown, out hit, 2f)) {
      if (hit.collider.gameObject.layer == groundLayerIndex) {
        transform.position = hit.point;
        transform.parent = null;
      }
    }
  }

  private void Release () {
    collisionEnabled = true;
    collider.enabled = true;

    // Only update height if the object is moving
    if (this.velocity > 0) {
      StartCoroutine (UpdateHeight ());
    }

    StartCoroutine (FadeOutAfterLifeEnd ());
  }

  private IEnumerator UpdateHeight () {
    int groundLayerIndex = LayerMask.NameToLayer ("Ground");

    while (true) {
      Ray rayDown = new Ray (transform.position, Vector3.down);
      RaycastHit hit;
      if (Physics.Raycast (rayDown, out hit, 2f)) {
        if (hit.collider.gameObject.layer == groundLayerIndex) {
          transform.position = new Vector3 (transform.position.x, hightFromTerrain + hit.point.y, transform.position.z);
        }
      }
      yield return null;
    }
  }

  private void OnTriggerStay (Collider other) {
    int otherLayer = other.gameObject.layer;

    if (collisionEnabled) {
      if (UnityExtensions.ContainsLayer (mask, otherLayer)) {
        Impact (transform.position);

        Debug.Log ("Meh");
        Explosive e;
        if ((e = other.GetComponent<Explosive> ()) != null) {
          e.Explode ();
          Debug.Log ("Heh");
        }
      }
    }
  }

  private IEnumerator FadeOutAfterLifeEnd () {
    yield return new WaitForSeconds (lifeTime);
    Fade ();
  }

  public void Fade () {
    DisableProjectile ();
    impactEndTimer = new CustomUpdateTimer (0.9f * impactEffect.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void DisableProjectile () {
    collisionEnabled = false;
    collider.enabled = false;
    Destroy (GetComponent<Rigidbody> ());
    GetComponent<ParticleSystem> ().Stop ();
  }

  private void Impact (Vector3 impactPosition) {
    DisableProjectile ();
    CheckHitsAndDealDamage ();

    GameObject impactEffectInstance = Instantiate (impactEffect, impactPosition, transform.rotation, transform);
    impactEffectInstance.transform.localScale = transform.localScale;

    impactEndTimer = new CustomUpdateTimer (0.9f * impactEffect.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void CheckHitsAndDealDamage () {
    Collider[] hits = Physics.OverlapSphere (transform.position, impactRadius);

    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    foreach (Collider hit in hits) {
      if (hit.gameObject.layer == enemyLayer) {
        hit.GetComponent<EnemyControls> ().Aggro (casterTransform);
        hit.GetComponent<EnemyStats> ().TakeDamage (damage);
      }
    }
  }

  private void OnDrawGizmosSelected () {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere (transform.position, impactRadius);
  }

}