using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CastleController : MonoBehaviour {

	[Header("Souls Effect")]
	public GameObject particleLower;
	public GameObject particleUpper;
	[Space]
	public bool isActive = true;

	public void UpdateSettings() {
		if(isActive) {
			particleLower.SetActive(true);
			particleUpper.SetActive(true);
		} else {
			particleLower.SetActive(false);
			particleUpper.SetActive(false);
		}
	}

}

#if UNITY_EDITOR

[CustomEditor (typeof(CastleController))]
public class CastleControllerEditor : Editor {

	public override void OnInspectorGUI() {
		CastleController controller = (CastleController)target;

		if(DrawDefaultInspector()) {
			controller.UpdateSettings();
		}
	}
}

#endif


