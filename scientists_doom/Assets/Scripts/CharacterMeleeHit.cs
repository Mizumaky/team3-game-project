using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMeleeHit : CharacterAbility {

	private void OnEnable() {
    //RaycastHit[] hits = Physics.SphereCastAll(attackSpawnPoint.position, radius, transform.forward, 0, sphereCastLayerMask);
    //for(int i = 0; i < hits.Length; i++) {
    //  print(hits[i].collider.gameObject.name + "was hit!");
    //}
    
    //gameObject.SetActive(false);
  }

  private void OnDrawGizmosSelected() {
    //Gizmos.color = Color.red;
    //Gizmos.DrawWireSphere(transform.position, radius);
  }

}
