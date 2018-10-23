using UnityEngine;
using UnityEngine.PostProcessing;

public class GraphicsQualityController : MonoBehaviour {

	[Header("Graphics Quality")]
	public GraphicsQualityPreset defaultPreset = GraphicsQualityPreset.Low;
	public enum GraphicsQualityPreset { Low, High }

	[Header("PostProcessing")]
	public PostProcessingBehaviour behaviour;
	public PostProcessingProfile low;
	public PostProcessingProfile high;

	private void Awake() {
		SetQualityPresetIndex((int)defaultPreset);
	}

	/// <summary>
	/// Sets graphics quality preset index
	/// </summary>
	/// <param name="value">Graphics preset index (0 - low, 1 - high)</param>
	public void SetQualityPresetIndex(int value) {
		defaultPreset = (GraphicsQualityPreset)value;
	}

	/// <summary>
	/// Updates current graphics quality and post processing preset
	/// </summary>
	public void UpdateQuality() {
		switch((int)defaultPreset) {
			default: 	behaviour.profile = low;
								QualitySettings.SetQualityLevel(0);
								break;

			case 1: behaviour.profile = high;
							QualitySettings.SetQualityLevel(5);
							break;
		}
	}
}
