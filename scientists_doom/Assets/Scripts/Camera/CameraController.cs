using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public enum Camera { PlayerDF, TurretFP }
    public static Camera currentCamera = Camera.PlayerDF;
    private Camera lastCamera;
    private DoubleFocusCamera dFCameraScript;
    private TurretCamera turretCameraScript;

    private void Start() {
        dFCameraScript = GetComponent<DoubleFocusCamera>();
        turretCameraScript = GetComponent<TurretCamera>();
    }

    void LateUpdate() {
        switch (currentCamera) {
            case (Camera.PlayerDF):
                if (lastCamera == Camera.TurretFP) {
                    turretCameraScript.SwitchFrom();
                    Debug.Log("switched from turret cam");
                }
                dFCameraScript.UpdateDFCamera();
                break;
            case (Camera.TurretFP):
                if (lastCamera != Camera.TurretFP) {
                    turretCameraScript.SwitchTo();
                    Debug.Log("switched to turret cam");
                }
                turretCameraScript.UpdateTurretCamera();
                break;
            default:
                dFCameraScript.UpdateDFCamera();
                break;
        }
        lastCamera = currentCamera;
    }
}
