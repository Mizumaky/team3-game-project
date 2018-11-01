using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

  protected PlayerStateController playerStateController;

  [Header ("Weapon")]
  [Space]
  public Transform weaponTransform;
  [SerializeField] protected float weaponDamage;
  [Space]
  protected bool isPlayerControlled;
  private void Awake () {
    Init ();
  }
  private void Update () {
    if (isPlayerControlled) {
      GetInput ();
    }
  }

  protected virtual void Init () {

    if (gameObject.layer == LayerMask.NameToLayer ("Player")) {
      isPlayerControlled = true;
      playerStateController = GetComponent<PlayerStateController> ();
    } else
      isPlayerControlled = false;
  }

  protected virtual void GetInput () {
    // Check focus layer
    if (GameController.currentFocusLayer == GameController.FocusLayer.Game) {

      // Check player state
      if (playerStateController != null) {
        if (playerStateController.currentState == PlayerStateController.PlayerState.movingState) {
          if (Input.GetKeyDown (KeyCode.Space)) {
            PerformBasicAttack ();
          }
        }
      } else {
        Debug.LogWarning ("No playerStateController set!");

      }
    }
  }

  protected virtual void PerformBasicAttack () {
    Debug.Log ("No basic attack found!");
  }

  protected float GetCurrentTotalDamage () {
    float damage = GetComponent<Stats> ().GetAttackDamage () + weaponDamage;
    return damage;
  }

}