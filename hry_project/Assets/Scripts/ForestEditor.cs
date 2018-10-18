#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(ForestGenerator))]
public class ForestEditor : Editor {

	public override void OnInspectorGUI() {
		ForestGenerator generator = (ForestGenerator)target;

		if(DrawDefaultInspector()) {

		}

		if(GUILayout.Button("Generate")) {
			generator.GenerateForest();
		}
	}
}

#endif

