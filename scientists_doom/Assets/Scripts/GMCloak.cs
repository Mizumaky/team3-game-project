using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMCloak : MonoBehaviour {

	public Transform armatureTransform;
	private Cloth cloakCloth;

	private void Awake () {
		cloakCloth = GetComponent<Cloth> ();
		CapsuleCollider[] cols = armatureTransform.GetComponentsInChildren<CapsuleCollider> ();
		cloakCloth.capsuleColliders = cols;
	}
}