using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private DoubleFocusCamera dFCameraScript;

    private void Start() {

        dFCameraScript = GetComponent<DoubleFocusCamera>();
    }

    void LateUpdate() {
        dFCameraScript.UpdateDFCamera();
    }

    //TODO implement methods for changing camera, other controllers will call em
}
