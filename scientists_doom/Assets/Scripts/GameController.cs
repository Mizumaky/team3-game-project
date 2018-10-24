using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

		public enum FocusLayer { Game, UI }

		[SerializeField] public static FocusLayer currentFocusLayer;

		public void LoadMainMenu() {
			SceneManager.LoadScene(0);
		}
}
