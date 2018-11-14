#define DEBUG

using System.Collections;
using UnityEngine;

public class CharacterAbilityInstance : MonoBehaviour {

  private Transform casterTransform;
  private float damage = 0f;
  private float lifeTime = 2f;
  private Vector3 destination;
  private bool canHit;

  [Header ("Physics")]
  private new Rigidbody rigidbody;
  private new Collider collider;
  [SerializeField] private LayerMask collisionMask;
  private float heightFromTerrain = 0.5f;
  [Space]

  [Header ("Light")]
  [SerializeField] private new Light light;
  private float initialLightIntensity;
  [Space]

  [Header ("End Effect")]
  [SerializeField] private bool hasEndEffect = false;
  [SerializeField] private GameObject endEffectPrefab;
  [SerializeField][Range (1f, 20f)] private float endEffectRadiusScale = 1f;
  [SerializeField] private float baseEndEffectRadius;
  [SerializeField] private float endEffectRadius;
  [SerializeField] private bool hasLifeTimeCountdownAfterLaunch;
  private CustomUpdateTimer endEffectLifeTimer;
  private CharacterAbility ability;

  #region GettersSetters
  public void SetAbility (CharacterAbility ability) {
    this.ability = ability;
  }
  public float GetImpactRadius () {
    return endEffectRadius;
  }
  #endregion

  private void Awake () {
    Init ();
    SetLight ();
  }

  private void Update () {
    EndEffectUpdate ();
    if (destination != Vector3.zero) {
      if (Vector3.Distance (transform.position, destination) < 0.5f) {
        DisableAndSpawnEndEffect (transform.position);
        destination = Vector3.zero;
      }
    }
  }

  private void Init () {
    // Collider
    rigidbody = GetComponent<Rigidbody> ();
    if ((collider = GetComponent<Collider> ()) != null) {
      collider.enabled = false;
    }

    light = GetComponent<Light> ();
  }

  private void SetLight () {
    if (light != null) {
      initialLightIntensity = light.intensity;
    }
  }

  private void EndEffectUpdate () {
    // Destroy GO after impact timer has ended
    if (endEffectLifeTimer != null) {
      if (endEffectLifeTimer.isLastTick (Time.deltaTime)) {
        Destroy (gameObject);
      } else {
        if (light != null) {
          light.intensity = initialLightIntensity * endEffectLifeTimer.GetTimeLeftPercent ();
        }
      }
    }
  }

  private void SetParameters (float weaponPlusStatDamage, float chargeFactor) {
    casterTransform = ability.GetCasterTransform ();
    transform.parent = null;

    if (ability.GetTravelDestination () == CharacterAbility.TravelDestination.air) {
      destination = GetScreenPointToRayHitPoint () + Vector3.up * ability.GetDestinationHeight ();
      rigidbody.velocity = (destination - transform.position).normalized * ability.GetVelocity () * chargeFactor;
    } else if (ability.GetTravelDestination () == CharacterAbility.TravelDestination.ground) {
      rigidbody.velocity = ability.GetWeaponTransform ().up * ability.GetVelocity () * chargeFactor;
    }

    // only increase ability dmg by charge
    damage = weaponPlusStatDamage + ability.GetDamage () * chargeFactor;

    baseEndEffectRadius = ability.GetBaseHitRadius ();
    endEffectRadius = baseEndEffectRadius * chargeFactor;

    lifeTime = ability.GetLifeTime ();
  }

  public void SetAndRelease (float weaponPlusStatDamage, float chargeFactor, CharacterAbility ability) {
    this.ability = ability;
#if DEBUG
    Debug.Log ("Releasing " + gameObject.name + "!");
#endif
    SetParameters (weaponPlusStatDamage, chargeFactor);

    canHit = true;
    if (collider != null)
      collider.enabled = true;

    if (ability.GetTravelDestination () == CharacterAbility.TravelDestination.ground) {
      if (!rigidbody.useGravity) {
        StartCoroutine (UpdateHeight ());
      }
    } else if (ability.GetTravelDestination () == CharacterAbility.TravelDestination.self) {
      DisableAndSpawnEndEffect (transform.position);
      return;
    }

    if (hasLifeTimeCountdownAfterLaunch) {
      StartCoroutine (FadeOutAfterLifeEnd ());
    }
  }

  private IEnumerator UpdateHeight () {
    int groundLayerIndex = LayerMask.NameToLayer ("Ground");

    while (true) {
      Ray rayDown = new Ray (transform.position, Vector3.down);
      RaycastHit hit;
      if (Physics.Raycast (rayDown, out hit, 4f)) {
        if (hit.collider.gameObject.layer == groundLayerIndex) {
          transform.position = new Vector3 (transform.position.x, heightFromTerrain + hit.point.y, transform.position.z);
        }
      }
      yield return null;
    }
  }

  public void SetAndDrop () {
    SetParameters (0, 0);

#if DEBUG
    Debug.Log ("Dropping " + gameObject.name + "!");
#endif

    canHit = false;
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

  private void OnTriggerStay (Collider other) {
    int otherLayer = other.gameObject.layer;

    if (canHit) {
      if (UnityExtensions.ContainsLayer (collisionMask, otherLayer)) {
        DisableAndSpawnEndEffect (transform.position);

        Explosive e = null;
        if ((e = other.GetComponent<Explosive> ()) != null) {
          e.Explode (casterTransform);
        }
      }
    }
  }

  private IEnumerator FadeOutAfterLifeEnd () {
    yield return new WaitForSeconds (lifeTime);
    Fade ();
  }

  public void Fade () {
    Disable ();
    endEffectLifeTimer = new CustomUpdateTimer (0.9f);
  }

  private void Disable () {
    canHit = false;
    if (collider != null)
      collider.enabled = false;
    Destroy (GetComponent<Rigidbody> ());
    GetComponent<ParticleSystem> ().Stop ();
  }

  private void DisableAndSpawnEndEffect (Vector3 impactPosition) {
    Disable ();

    GameObject endEffectInstance = Instantiate (endEffectPrefab, impactPosition, transform.rotation, transform);

    float hitScale = (float) endEffectRadius / (float) baseEndEffectRadius;
#if DEBUG
    Debug.Log ("End effect radius : " + endEffectRadius + "!");
#endif

    // TODO: WTF
    Hit (endEffectRadius);

    endEffectInstance.transform.localScale = new Vector3 (hitScale, hitScale, hitScale);
    foreach (Transform transform in endEffectInstance.transform.GetComponentsInChildren<Transform> ()) {
      transform.localScale = new Vector3 (hitScale, hitScale, hitScale);
    }
    Debug.Log ("End effect: " + 0.9f * endEffectPrefab.GetComponent<ParticleSystem> ().main.duration);
    endEffectLifeTimer = new CustomUpdateTimer (0.9f * endEffectPrefab.GetComponent<ParticleSystem> ().main.duration);
  }

  private void Hit (float hitRadius) {
    Collider[] hits = Physics.OverlapSphere (transform.position, hitRadius, collisionMask);

    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    foreach (Collider hit in hits) {
#if DEBUG
      Debug.Log ("Hitting!");
#endif

      if (hit.gameObject.layer == enemyLayer) {
        hit.GetComponent<EnemyControls> ().Aggro (casterTransform);
        if (ability.GetStunDuration () > 0) {
          StartCoroutine (hit.GetComponent<EnemyControls> ().StunFor (ability.GetStunDuration ()));
        }
        hit.GetComponent<EnemyStats> ().TakeDamage (damage);
      }
    }
  }

  private void OnDrawGizmosSelected () {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere (transform.position, endEffectRadius);
  }

  private Vector3 GetScreenPointToRayHitPoint () {
    Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    RaycastHit hit;

    int groundLayer = 1 << LayerMask.NameToLayer ("Ground");

    if (Physics.Raycast (ray, out hit, 100f, groundLayer)) {
      return hit.point;
    }
    return transform.position;
  }
}