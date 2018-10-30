using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

  [Header ("Weapon")]
  [Space]
  public Transform weaponTransform;
  [SerializeField] protected int weaponDamage;
  [Space]
  private Object nothing;

  private void Update () {
    GetInput ();
  }

  protected virtual void GetInput () {
    if (Input.GetKeyDown (KeyCode.Space)) {
      PerformBasicAttack ();
    }
  }

  protected virtual void PerformBasicAttack () {

  }

}