using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public GameObject settingsPanel;
	private GameController controller;

	public void Awake() {
		controller = GetComponent<GameController>();
	}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				ToggleSettingsWindow();
			}
		}

	public void ToggleSettingsWindow() {
		if(settingsPanel.activeSelf == true) {
			settingsPanel.SetActive(false);
			controller.currentFocusLayer = GameController.FocusLayer.Game;
		} else {
			controller.currentFocusLayer = GameController.FocusLayer.UI;
			settingsPanel.SetActive(true);
		}
	}

	public void OpenSGWebsite() {
		Application.OpenURL("http://scarcegames.com/");
	}
}
