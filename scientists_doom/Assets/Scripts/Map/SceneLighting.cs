using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLighting : MonoBehaviour {

	public Camera mycamera;

	[Header ("Light Settings")]
	public Light sunLight;
	public Light moonLight;
	public float intensity;

	public float fogDensity = 0.02f;

	[Header ("Progressive Light")]
	public Gradient sunlightGradient;
	public Gradient moonlightGradient;
	public Gradient backgroundGradient;
	[Range (0, 1f)] public float progress = 0f;
	public float progressSpeed = 0.1f;

	private void Awake () {
		RenderSettings.fog = true;
	}

	private void Update () {
		sunLight.intensity = intensity;
		moonLight.intensity = intensity;
		RenderSettings.fogDensity = fogDensity;

		UpdateShiftingLight ();
	}

	public void UpdateShiftingLight () {
		progress = (progress + progressSpeed * Time.deltaTime) % 1;
		sunLight.color = sunlightGradient.Evaluate (progress);
		moonLight.color = moonlightGradient.Evaluate (progress);
		mycamera.backgroundColor = backgroundGradient.Evaluate (progress);
		RenderSettings.fogColor = backgroundGradient.Evaluate (progress);

		sunLight.transform.localRotation = Quaternion.Euler (new Vector3 (360f * progress, 0, 0));
		moonLight.transform.localRotation = Quaternion.Euler (new Vector3 (180f + 360f * progress, 0, 0));
	}
}