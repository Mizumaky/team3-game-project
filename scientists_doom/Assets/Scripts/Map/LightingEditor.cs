#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(SceneLighting))]
public class LightingEditor : Editor {

	public override void OnInspectorGUI() {
		SceneLighting lighting = (SceneLighting)target;

		if(DrawDefaultInspector()) {
			lighting.UpdateLighting();
		}
	}
}

#endif

