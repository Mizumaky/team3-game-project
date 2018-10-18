using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void StartSinglePlayer() {

        SceneManager.LoadScene("SinglePlayerGame");

    }

    public void GoToLobby() {

        SceneManager.LoadScene("Lobby");

    }

    public void GoToSettings() {

        //Open Settings Menu

    }

    public void QuitGame() {

        Application.Quit();

    }
	
}
