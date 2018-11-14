//#define DEBUG

// TODO: Fix abilities being able to trigger each other

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
  protected bool isPlayerControlled;
  protected PlayerStateController playerStateController;
  private Animator characterAnimator;

  [Header ("Weapon Parameters")]
  [SerializeField] protected Transform weaponTransform;
  [SerializeField] protected float weaponDamage;
  [SerializeField] protected float castSpeed;
  [Space]

  [Header ("Abilities")]
  [SerializeField] protected CharacterAbility regularAttack;
  [SerializeField] protected CharacterAbility ability1;
  [SerializeField] protected CharacterAbility ability2;
  [Space]

  [Header ("Current")]
  [SerializeField] protected CharacterAbility currentAbility;
  [SerializeField] protected GameObject currentAbilityObject;
  [SerializeField] protected float chargeTime;
  [SerializeField] protected float chargeFactor = 0;
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
      // Ability 2
      if (Input.GetKeyDown (KeyCode.E))
        PerformAbility (ability2);
    }
    if (chargeUpdateRoutine != null) {
      // Regular
      if (Input.GetKeyUp (KeyCode.Space) && currentAbility == regularAttack) {
        ReleaseAbility (regularAttack);

      }
      // Ability 1
      if (Input.GetKeyUp (KeyCode.Q) && currentAbility == ability1) {
        ReleaseAbility (ability1);
      }
      // Ability 2
      if (Input.GetKeyUp (KeyCode.E) && currentAbility == ability2)
        ReleaseAbility (ability2);
    }
  }

  protected virtual void PerformAbility (CharacterAbility ability) {
    ability.Cast (transform, weaponTransform);

    if (chargeUpdateRoutine != null) {
      ReleaseAbility (ability);
    }
    if (characterAnimator != null) {
      characterAnimator.SetTrigger (ability.GetAnimationTrigger ());
    }

    currentAbility = ability;
    if (ability.hasInstance ()) {
      currentAbilityObject = Instantiate (ability.GetAbilityPrefab (), weaponTransform.position, transform.rotation, weaponTransform) as GameObject;
      currentAbilityObject.GetComponent<CharacterAbilityInstance> ().SetAbility (ability);
    }

    if (ability.GetMaxChargeTime () == 0) {
      ReleaseAbility (ability);
    } else {
      chargeUpdateRoutine = StartCoroutine (UpdateCurrentlyCharged (ability));
    }
  }

  protected virtual void ReleaseAbility (CharacterAbility ability) {
    float statPlusWeaponDamage = GetCurrentStatPlusWeaponDamage () + ability.GetDamage ();

    if (ability.hasInstance ()) {
      CharacterAbilityInstance instance = currentAbilityObject.GetComponent<CharacterAbilityInstance> ();
      if (chargeTime < ability.GetChargeStartDelay ()) {
        if (ability.DropsDuringStartDelay ()) {
          instance.SetAndDrop ();
        } else {
          instance.Fade ();
        }
      } else {
        instance.SetAndRelease (GetCurrentStatPlusWeaponDamage (), chargeFactor, ability);
      }
    }

    ResetCurrentlyCharged ();
  }

  protected void ResetCurrentlyCharged () {
    if (chargeUpdateRoutine != null) {
      StopCoroutine (chargeUpdateRoutine);
      chargeUpdateRoutine = null;
    }

    currentAbilityObject = null;
    chargeFactor = 0;
    chargeTime = 0;
  }

  private IEnumerator UpdateCurrentlyCharged (CharacterAbility ability) {
    float maxChargeTime = ability.GetMaxChargeTime ();
    float chargeSpeed = ability.GetChargeSpeed ();
    float chargeStartDelay = ability.GetChargeStartDelay ();
    float startChargeFactor = ability.GetStartChargeFactor ();
    chargeFactor = startChargeFactor;

    Vector3 scale = ability.GetAbilityPrefab ().transform.localScale;

    while (chargeTime < maxChargeTime + chargeStartDelay) {
      chargeTime += Time.deltaTime;
      if (chargeTime > chargeStartDelay) {
        chargeFactor = chargeFactor + Time.deltaTime * chargeSpeed;

        if (currentAbilityObject != null) {
          currentAbilityObject.transform.localScale = scale * chargeFactor;
        }
      }
      yield return null;
    }

    if (releaseOnMaxCharge) {
      ReleaseAbility (ability);
    }
  }

  public float GetCurrentStatPlusWeaponDamage () {
    float currentTotalDamage = GetComponent<Stats> ().GetAttackDamage () + weaponDamage;
    return currentTotalDamage;
  }
}