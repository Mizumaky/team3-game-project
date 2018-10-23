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

	/// <summary>
	/// Sets quality level and post processing profile
	/// </summary>
	/// <param name="value">Graphics preset index (0 - low, 1 - high)</param>
	public void SetQuality(int value) {
		switch(value) {
			// Low == Default
			default: behaviour.profile = low;
							QualitySettings.SetQualityLevel(0);
							break;

			// High
			case 2: behaviour.profile = high;
							QualitySettings.SetQualityLevel(5);
							break;
		}
	}
}
