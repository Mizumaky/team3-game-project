using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

  public Transform casterTransform;
	public float lifetime = 5f;

	void Start () {
		StartCoroutine(AutodestroyCountdown());
	}

  private void OnCollisionEnter(Collision other) {
    if(other.gameObject.layer == 10) { // enemy layer
      Destroy(gameObject);
    }
  }
	
	IEnumerator AutodestroyCountdown() {
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}
}
