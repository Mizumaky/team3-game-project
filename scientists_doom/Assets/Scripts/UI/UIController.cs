using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages UI windows
/// </summary>
public class UIController : MonoBehaviour {

	public GameController gameController;
	public CharacterManager characterManager;
	public EnemySpawner enemySpawner;

	[Header ("Windows")]
	public GameObject currentWindow;
	[Space]
	public GameObject settingsWindow;
	public GameObject characterWindow;

	[Header ("UI Elements")]
	[Header ("Settings Window")]
	public Dropdown settingsQualityDropdown;

	private void Awake () {
		if (characterWindow.activeSelf) {
			currentWindow = characterWindow;
		}
	}

	private void Update () {
		GetUIInput ();
	}

	private void GetUIInput () {
		// ESC
		if (Input.GetKeyDown (KeyCode.Escape)) { // close any window or open settings
			if (currentWindow == null) {
				ToggleWindow (settingsWindow);
			} else {
				ToggleWindow (currentWindow);
			}
		}

		// C
		if (Input.GetKeyDown (KeyCode.C)) {
			ToggleWindow (characterWindow);
		}
	}

	/// <summary>
	/// Toggles a UI window, only one can be enabled at a time
	/// </summary>
	/// <param name="window">Window to toggle</param>
	public void ToggleWindow (GameObject window) {
		if (currentWindow == null) { // no window active
			GameController.currentFocusLayer = GameController.FocusLayer.UI;
			currentWindow = window;
			currentWindow.SetActive (true);
		} else {
			if (window == currentWindow) { // closing an active window
				GameController.currentFocusLayer = GameController.FocusLayer.Game;
				currentWindow.SetActive (false);
				currentWindow = null;
			} else { // closing one and opening another
				currentWindow.SetActive (false);
				currentWindow = window;
				currentWindow.SetActive (true);
			}
		}
	}

	/// <summary>
	/// Changes quality via GraphicsQualityController
	/// </summary>
	public void ChangeQualitySettings () {
		gameController.GetComponent<GraphicsQualityController> ().SetQualityPresetIndex (settingsQualityDropdown.value);
		gameController.GetComponent<GraphicsQualityController> ().UpdateQuality ();
	}

	/// <summary>
	/// Changes active character via CharacterManager
	/// </summary>
	/// <param name="index"></param>
	public void ChangeActiveCharacter (int index) {
		characterManager.ChangeActiveCharacter (index);
		ToggleWindow (characterWindow);
	}

	public void SpawnNewEnemyWave () {
		enemySpawner.StartSpawnWaveIfInactive ();
	}

	/// <summary>
	/// Opens scarcegames website in the default web browser
	/// </summary>
	public void OpenSGWebsite () {
		Application.OpenURL ("http://scarcegames.com/");
	}

	public void LoadMainMenu () {
		SceneManager.LoadScene (0);
	}
}