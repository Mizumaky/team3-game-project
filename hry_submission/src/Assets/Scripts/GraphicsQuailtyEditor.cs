#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(GraphicsQualityController))]
public class GraphicsQuailtyEditor : Editor {

	public override void OnInspectorGUI() {
		GraphicsQualityController controller = (GraphicsQualityController)target;

		if(DrawDefaultInspector()) {
			controller.UpdateQuality();
		}
	}
}

#endif

