using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WizardWeapon : Weapon {

  [Header ("Regular Attack")]
  [Space]
  public GameObject fireballPrefab;
  public float fireballMaxChargeTime = 1f;
  public float fireballChargeSpeed = 1.5f;
  [Space]

  private GameObject currentlyCharged;
  private float currentChargeTime;

  private void Update () {
    GetInput ();
    if (currentlyCharged != null) {
      UpdateCurrentlyCharged ();
    }
  }
  protected override void GetInput () {
    base.GetInput ();
    // Check focus layer
    if (GameController.currentFocusLayer == GameController.FocusLayer.Game) {

      // Check player state
      if (playerStateController != null) {
        if (playerStateController.currentState == PlayerStateController.PlayerState.movingState) {
          if (Input.GetKeyUp (KeyCode.Space)) {
            if (currentlyCharged != null) {
              ReleaseBasicAttack ();
            }
          }
        }
      } else {
        Debug.LogWarning ("No playerStateController set!");
      }
    }

  }

  protected override void PerformBasicAttack () {
    currentlyCharged = Instantiate (fireballPrefab, weaponTransform.position, weaponTransform.rotation, transform) as GameObject;
    currentlyCharged.GetComponent<CharacterAbility> ().casterTransform = transform;
  }

  protected void ReleaseBasicAttack () {
    currentlyCharged.transform.parent = null;
    currentlyCharged.GetComponent<CharacterAbility> ().damage = weaponDamage + GetComponent<PlayerStats> ().GetAttackDamage () * currentChargeTime * fireballChargeSpeed;
    currentlyCharged.GetComponent<CharacterProjectile> ().impactRadius *= (1 + currentChargeTime * fireballChargeSpeed);
    currentlyCharged.GetComponent<Rigidbody> ().velocity = currentlyCharged.transform.up * currentlyCharged.GetComponent<CharacterProjectile> ().velocity * (1 + currentChargeTime * fireballChargeSpeed) / 2f;
    currentlyCharged.GetComponent<CharacterProjectile> ().Release ();
    ResetCurrentlyCharged ();
  }

  protected void UpdateCurrentlyCharged () {
    if (currentChargeTime < fireballMaxChargeTime) {
      currentChargeTime += Time.deltaTime * fireballChargeSpeed;
      currentlyCharged.transform.localScale = fireballPrefab.transform.localScale * (1 + currentChargeTime * fireballChargeSpeed);
    } else {
      ReleaseBasicAttack ();
    }
  }

  protected void ResetCurrentlyCharged () {
    currentlyCharged = null;
    currentChargeTime = 0;
  }

}