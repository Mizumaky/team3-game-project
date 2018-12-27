using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
}
