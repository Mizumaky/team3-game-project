using System.Collections;
using UnityEngine;

public class CharacterAbilityInstance : MonoBehaviour {

  [Header ("Carry")]
  [SerializeField] protected Transform casterTransform;
  [SerializeField] protected float damage;
  [Space]

  [Header ("Object Settings")]
  [SerializeField] protected Rigidbody rigidbody;
  [SerializeField] protected LayerMask mask;
  [SerializeField] protected float velocity = 0f;
  [SerializeField] protected float lifeTime = 2f;
  [SerializeField] protected float hightFromTerrain = 0.5f;
  [Space]
  [SerializeField] protected new Light light;
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
    initialLightIntensity = light.intensity;
  }

  private void Update () {
    // Destroy GO after impact timer has ended
    if (impactEndTimer != null) {
      if (impactEndTimer.isLastTick (Time.deltaTime)) {
        Destroy (gameObject);
      } else {
        light.intensity = initialLightIntensity * impactEndTimer.GetTimeLeftPercent ();
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

  private void Release () {
    collisionEnabled = true;

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

  private void OnTriggerEnter (Collider other) {
    int otherLayer = other.gameObject.layer;

    if (collisionEnabled) {
      if (UnityExtensions.ContainsLayer (mask, otherLayer)) {
        Impact (transform.position);
      }
    }
  }

  private IEnumerator FadeOutAfterLifeEnd () {
    yield return new WaitForSeconds (lifeTime);
    collisionEnabled = false;

    DisableProjectile ();

    impactEndTimer = new CustomUpdateTimer (0.9f * impactEffect.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void DisableProjectile () {
    collisionEnabled = false;
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