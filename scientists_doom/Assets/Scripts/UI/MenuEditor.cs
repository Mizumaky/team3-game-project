#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor (typeof(MenuController))]
public class MenuEditor : Editor {

	public override void OnInspectorGUI() {
		MenuController controller = (MenuController)target;

		if(DrawDefaultInspector()) {

		}

		if(GUILayout.Button("Update BuildSettings")) {
			controller.AddScenesToBuildSettings();
		}
	}
}

#endif

