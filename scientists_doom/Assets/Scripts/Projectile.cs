using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float lifetime = 5f;

	void Start () {
		StartCoroutine(DestroyCountdown());
	}
	
	IEnumerator DestroyCountdown() {
		yield return new WaitForSeconds(lifetime);
		Destroy(gameObject);
	}
}
