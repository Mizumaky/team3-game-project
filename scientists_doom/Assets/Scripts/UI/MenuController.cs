using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void StartSinglePlayer() {

        SceneManager.LoadScene(2);

    }

    public void GoToLobby() {

        SceneManager.LoadScene(1);

    }

    public void GoToSettings() {

        //Open Settings Menu

    }

    public void QuitGame() {

        Application.Quit();

    }
	
}
