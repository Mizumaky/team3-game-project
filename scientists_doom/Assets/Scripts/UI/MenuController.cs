using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuController : MonoBehaviour {

    public enum User { Master, Petr, Mira, Fanda }
    public User currentUser = User.Petr;

    [Header("Paths To Scenes")]
    [Header("Scene names can be adjusted in script!")]
    public string master = "Assets/Scenes/";
    public string petr = "Assets/SceneCopies/petr/";
    public string mira = "Assets/SceneCopies/mira/";
    public string fanda = "Assets/SceneCopies/fanda/";

    public struct SceneNames {
        public string mainMenu;
        public string single;

        /// <summary>
        /// Default scene names for different users
        /// </summary>
        /// <param name="currentUser">current user</param>
        public SceneNames(User currentUser) {
            if(currentUser == User.Master) {
                mainMenu = "MainMenu.unity";
                single = "SinglePlayerGame.unity";
            } else {
                mainMenu = "MainMenu 1.unity";
                single = "SinglePlayerGame 1.unity";
            }
        }
    }

    public string GetCurrentUserPath() {
        switch(currentUser) {
            default: return master;
            case User.Petr: return petr;
            case User.Mira: return mira;
            case User.Fanda: return fanda;
        }
    }

    public void StartSinglePlayer() {
        SceneManager.LoadScene(1);
    }

    public void GoToSettings() {

        //Open Settings Menu

    }

    public void QuitGame() {
        Application.Quit();
    }

    /// <summary>
	/// Opens scarcegames website in the default web browser
	/// </summary>
	public void OpenSGWebsite() {
		Application.OpenURL("http://scarcegames.com/");
	}

#if UNITY_EDITOR
    /// <summary>
    /// Adds mandatory scenes to build settings
    /// </summary>
    public void AddScenesToBuildSettings()
    {
        SceneNames names = new SceneNames(currentUser);
        EditorBuildSettingsScene[] newScenes = { 
            new EditorBuildSettingsScene(GetCurrentUserPath() + names.mainMenu, true),
            new EditorBuildSettingsScene(GetCurrentUserPath() + names.single, true),
            };
        EditorBuildSettings.scenes = newScenes;
    }
#endif
}
