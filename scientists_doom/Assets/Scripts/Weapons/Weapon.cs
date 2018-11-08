//#define DEBUG

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
  [SerializeField] protected CharacterAbility currentAbility;
  [SerializeField] protected GameObject currentAbilityInstance;
  [SerializeField] protected float chargeTime;
  [SerializeField] protected float chargeScalingFactor = 0;
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
    if (chargeTime <= 0) {
      // Regular
      if (Input.GetKeyDown (KeyCode.Space))
        PerformAbility (regularAttack);
      // Ability 1
      if (Input.GetKeyDown (KeyCode.Q))
        PerformAbility (ability1);
    }
    if (chargeUpdateRoutine != null) {
      // Regular
      if (Input.GetKeyUp (KeyCode.Space)) {
        ReleaseAbility (regularAttack);
      }
      // Ability 1
      if (Input.GetKeyUp (KeyCode.Q)) {
        ReleaseAbility (ability1);
      }
    }
  }

  protected virtual void PerformAbility (CharacterAbility ability) {
    if (chargeUpdateRoutine != null) {
      ReleaseAbility (ability);
    }
    if (characterAnimator != null) {
      characterAnimator.SetTrigger (ability.GetAnimationTrigger ());
    }

#if DEBUG
    Debug.Log ("Casting " + ability.GetName () + "!");
#endif

    currentAbility = ability;
    if (ability.hasInstance ()) {
      currentAbilityInstance = Instantiate (ability.GetAbilityPrefab (), weaponTransform.position, weaponTransform.rotation, weaponTransform) as GameObject;
    }

    if (ability.GetMaxChargeTime () == 0) {
      ReleaseAbility (ability);
    } else {
      chargeUpdateRoutine = StartCoroutine (UpdateCurrentlyCharged (ability));
    }
  }

  protected virtual void ReleaseAbility (CharacterAbility ability) {
    Vector3 velocity = transform.forward * 5f * (chargeScalingFactor + 1);
    float damage = (GetCurrentStatPlusWeaponDamage () + ability.GetDamage ()) * (chargeScalingFactor + 1);
    float impactRadius = currentAbilityInstance.GetComponent<CharacterAbilityInstance> ().GetImpactRadius () * (chargeScalingFactor + 1);

    CharacterAbilityInstance instance = currentAbilityInstance.GetComponent<CharacterAbilityInstance> ();

    if (chargeTime < ability.GetChargeStartDelay ()) {
      if (ability.DropsDuringStartDelay ()) {
        instance.Drop ();
      } else {
        instance.Fade ();
      }
    } else {
      instance.SetAndRelease (transform, velocity, damage, impactRadius);
    }

    ResetCurrentlyCharged ();
  }

  protected void ResetCurrentlyCharged () {
    if (chargeUpdateRoutine != null) {
      StopCoroutine (chargeUpdateRoutine);
      chargeUpdateRoutine = null;
    }

    currentAbilityInstance = null;
    chargeScalingFactor = 0;
    chargeTime = 0;
  }

  private IEnumerator UpdateCurrentlyCharged (CharacterAbility ability) {
    float maxChargeTime = ability.GetMaxChargeTime ();
    float chargeSpeed = ability.GetChargeSpeed ();
    float chargeStartDelay = ability.GetChargeStartDelay ();

    Vector3 scale = ability.GetAbilityPrefab ().transform.localScale;

    while (chargeTime < maxChargeTime + chargeStartDelay) {
      chargeTime += Time.deltaTime;
      if (chargeTime > chargeStartDelay) {
        chargeScalingFactor = chargeScalingFactor + Time.deltaTime * chargeSpeed;

        if (currentAbilityInstance != null) {
          currentAbilityInstance.transform.localScale = scale * (chargeScalingFactor + 1);
        }
      }
      yield return null;
    }

    if (releaseOnMaxCharge) {
      ReleaseAbility (ability);
    }
  }

  public float GetCurrentStatPlusWeaponDamage () {
    float currentTotalDamage = GetComponent<Stats> ().GetAttackDamage () + damage;
    return currentTotalDamage;
  }

}