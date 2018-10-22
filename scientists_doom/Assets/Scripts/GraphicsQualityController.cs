using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class GraphicsQualityController : MonoBehaviour {

	public Dropdown dropdown;
	public PostProcessingBehaviour behaviour;
	public PostProcessingProfile low;
	public PostProcessingProfile high;

	public void SetPostProcessingProfile() {
		switch (dropdown.value) {
			case 0: behaviour.profile = low; 
              QualitySettings.SetQualityLevel(0);
              break;
			case 1: behaviour.profile = high; 
              QualitySettings.SetQualityLevel(5);
              break;
			default: break;
		}
	}
}
