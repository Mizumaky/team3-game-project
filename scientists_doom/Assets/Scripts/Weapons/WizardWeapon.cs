using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WizardWeapon : Weapon {

  private void Awake () {
    Init ();
  }

  private void Update () {
    if (isPlayerControlled) {
      GetInput ();
    }
  }

}