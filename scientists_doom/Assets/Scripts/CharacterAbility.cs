using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour {

  public Transform casterTransform;
  public float damage;
  protected bool isPlayerControlled;

  private void Awake () {
    if (gameObject.layer == LayerMask.NameToLayer ("Player"))
      isPlayerControlled = true;
    else
      isPlayerControlled = false;
  }

}