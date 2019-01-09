using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {

    public Text text;
    float deltaTime = 0.0f;

    void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        text.text = ((int) (1 / deltaTime)).ToString ();
    }

}