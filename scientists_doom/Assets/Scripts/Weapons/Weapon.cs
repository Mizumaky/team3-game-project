using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
  protected bool isPlayerControlled;
  protected PlayerStateController playerStateController;
  private Animator characterAnimator;

  [Header ("Weapon Parameters")]
  [SerializeField] protected Transform weaponTransform;
  [SerializeField] protected float damage;
  [SerializeField] protected float castSpeed;
  [Space]

  [Header ("Abilities")]
  [SerializeField] protected CharacterAbility regularAttack;
  [SerializeField] protected CharacterAbility ability1;
  [SerializeField] protected CharacterAbility ability2;
  [Space]

  [Header ("Current")]
  [SerializeField] protected GameObject currentAbilityInstance;
  [SerializeField] protected float chargeTime;
  [SerializeField] protected float chargeScalingFactor = 1;
  [SerializeField] protected bool releaseOnMaxCharge = true;
  protected Coroutine chargeUpdateRoutine;

  private void Awake () {
    Init ();
  }

  private void Update () {
    if (isPlayerControlled) {

      // If state controller is present, prevent attacking while in a turret
      if (playerStateController != null) {
        if (playerStateController.currentState == PlayerStateController.PlayerState.movingState) {
          GetInput ();
        }
      } else {
        GetInput ();
      }
    }
  }

  protected virtual void Init () {
    if ((characterAnimator = GetComponent<Animator> ()) == null) {
      Debug.LogWarning ("No animator attached!");
    }

    if (gameObject.layer == LayerMask.NameToLayer ("Player")) {
      isPlayerControlled = true;
      if ((playerStateController = GetComponent<PlayerStateController> ()) == null) {
        Debug.LogWarning ("No player state controller attached!");
      }
    } else
      isPlayerControlled = false;
  }

  protected virtual void GetInput () {
    // Regular
    if (Input.GetKeyDown (KeyCode.Space)) {
      PerformRegularAttack ();
    } else if (Input.GetKeyUp (KeyCode.Space)) {
      if (chargeUpdateRoutine != null) {
        ReleaseRegularAttack ();
      }
    }
  }

  protected virtual void PerformRegularAttack () {
    if (chargeUpdateRoutine != null) {
      ReleaseRegularAttack ();
    }
    if (characterAnimator != null) {
      characterAnimator.SetTrigger ("attackTrigger");
    }

    Debug.Log ("Casting " + regularAttack.GetName () + "!");
    if (regularAttack.hasInstance ()) {
      currentAbilityInstance = Instantiate (regularAttack.GetAbilityPrefab (), weaponTransform.position, weaponTransform.rotation, weaponTransform) as GameObject;
    }

    if (regularAttack.GetMaxChargeTime () == 0) {
      ReleaseRegularAttack ();
    } else {
      chargeUpdateRoutine = StartCoroutine (UpdateCurrentlyCharged ());
    }
  }

  protected virtual void ReleaseRegularAttack () {
    Vector3 velocity = transform.forward * 5f * chargeScalingFactor;
    float damage = (GetCurrentStatPlusWeaponDamage () + regularAttack.GetDamage ()) * chargeScalingFactor;
    float impactRadius = currentAbilityInstance.GetComponent<CharacterAbilityInstance> ().GetImpactRadius () * chargeScalingFactor;

    currentAbilityInstance.GetComponent<CharacterAbilityInstance> ().SetAndRelease (transform, velocity, damage, impactRadius);

    ResetCurrentlyCharged ();
  }

  protected void ResetCurrentlyCharged () {
    if (chargeUpdateRoutine != null) {
      StopCoroutine (chargeUpdateRoutine);
      chargeUpdateRoutine = null;
    }

    currentAbilityInstance = null;
    chargeScalingFactor = 1;
    chargeTime = 0;
  }

  private IEnumerator UpdateCurrentlyCharged () {
    float maxChargeTime = regularAttack.GetMaxChargeTime ();
    float chargeSpeed = regularAttack.GetChargeSpeed ();

    Vector3 scale = regularAttack.GetAbilityPrefab ().transform.localScale;

    while (chargeTime < maxChargeTime) {
      chargeTime += Time.deltaTime;

      chargeScalingFactor = chargeScalingFactor + Time.deltaTime * chargeSpeed;

      if (currentAbilityInstance != null) {
        currentAbilityInstance.transform.localScale = scale * chargeScalingFactor;
      }

      yield return null;
    }

    if (releaseOnMaxCharge) {
      ReleaseRegularAttack ();
    }
  }

  public float GetCurrentStatPlusWeaponDamage () {
    float currentTotalDamage = GetComponent<Stats> ().GetAttackDamage () + damage;
    return currentTotalDamage;
  }

}