using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLighting : MonoBehaviour {

	public enum TimeOfDay { Morning, Midday, Evening, Midnight }
	public enum LightType { Static, Dynamic }

	public Camera mycamera;

	[Header("Light Settings")]
	public Light sceneLight;
	public float intensity;
	public TimeOfDay timeOfday = TimeOfDay.Midday;
	public LightType lightType = LightType.Dynamic;
	[Range(0.001f, 0.2f)]
	public float DLUpdateInterval = 0.2f;

	[Header("Scene Light Colors")]
	public Color morningColor = new Color(0.81f, 0.41f, 0.44f);
	public Color daylightColor = new Color(0.26f, 0.55f, 0.76f);
	public Color eveningColor = new Color(0.81f, 0.41f, 0.44f);
	public Color moonlightColor = new Color(0.11f, 0.43f, 0.81f);

	[Header("Atmosphere Colors")]
	public Color dayAtmosphereColor;
	public Color nightAtmosphereColor;

	[Header("Fog and Background")]
	public Color dayBackgroundColor;
	public Color nightBackgroundColor;
	[Range(0, 0.03f)]
	public float fogDensity = 0.02f;

	private Coroutine activeLightShift;

	private void Awake() {
		RenderSettings.fog = true;
	}

	private void Update() {
		UpdateLighting();
	}

	public void UpdateLighting() {
		sceneLight.intensity = intensity;

		if(activeLightShift == null) {
			activeLightShift = StartCoroutine(lightShift());
		}

		switch(timeOfday) {
			case TimeOfDay.Morning: 
				sceneLight.color = morningColor;
				mycamera.backgroundColor = dayBackgroundColor;
				RenderSettings.fogColor = dayBackgroundColor;
				break;
			case TimeOfDay.Midday:
				sceneLight.color = daylightColor;
				mycamera.backgroundColor = dayBackgroundColor;
				RenderSettings.fogColor = dayBackgroundColor;
				break;
			case TimeOfDay.Evening:
				sceneLight.color = eveningColor;
				mycamera.backgroundColor = nightBackgroundColor;
				RenderSettings.fogColor = nightBackgroundColor;
				break;
			case TimeOfDay.Midnight:
				sceneLight.color = moonlightColor;
				mycamera.backgroundColor = nightBackgroundColor;
				RenderSettings.fogColor = nightBackgroundColor;
				break;
			default:
				break;
		}
		RenderSettings.fogDensity = fogDensity;
	}

	private IEnumerator lightShift() {
		while(lightType == LightType.Dynamic) {
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0.05f, 0));
			yield return new WaitForSeconds(DLUpdateInterval);
		}
	}
}
