using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCamera : MonoBehaviour {

    private Transform previousCameraParent;
    private Transform camPoint;

    //implement somehow changing of cam position right on switch
    public void SwitchTo() {
        camPoint = TurretManager.activeTurretObject.transform.GetChild(0).Find("RotationPoint").Find("CameraPoint");
        transform.position = camPoint.position;
        transform.rotation = camPoint.rotation;
        previousCameraParent = transform.parent;
        transform.parent = camPoint;
    }

    public void SwitchFrom() {
        transform.parent = previousCameraParent;
    }

    public void UpdateTurretCamera() {
        

    }
}
