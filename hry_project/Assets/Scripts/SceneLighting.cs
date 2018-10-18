using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLighting : MonoBehaviour {

	public Camera mycamera;
	public Color daylightColor = new Color(0.26f, 0.55f, 0.76f);
	public Color morningEveningColor = new Color(0.81f, 0.41f, 0.44f);
	public Color moonlightColor = new Color(0.11f, 0.43f, 0.81f);

	public Color atmoshphereColor;
	[Range(0, 0.03f)]
	public float fogDensity = 0.02f;

	private void Start() {
		RenderSettings.fog = true;
	}

	private void Update() {
    mycamera.backgroundColor = atmoshphereColor;
		RenderSettings.fogColor = atmoshphereColor;
		RenderSettings.fogDensity = fogDensity;
	}

	public void UpdateLighting() {
		mycamera.backgroundColor = atmoshphereColor;
		RenderSettings.fogColor = atmoshphereColor;
		RenderSettings.fogDensity = fogDensity;
	}
}
