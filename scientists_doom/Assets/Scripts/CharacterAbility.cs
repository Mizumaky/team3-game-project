using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour {

  public Transform casterTransform;

  protected virtual void OnCollisionEnter(Collision other) {
    if(other.gameObject.layer == 10) { // enemy layer
      Destroy(gameObject);
    }
  }
}
