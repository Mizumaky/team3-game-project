using System.Collections;
using UnityEngine;

//TODO wizard and barbarian have different classes for attack,
//would like to make it work similiarily as for movement

public class PlayerController : MonoBehaviour {

    public GameObject associatedTurret;
 //   public Transform playerPositionAtTurret;
    protected PlayerMovement movementScript;
    protected TurretManager turretManagerScript;
    protected PlayerTurretControls turretControlsScript;

    protected enum PlayerState
    {
        movingState,
        turretControlState
    }
    protected PlayerState currentState = PlayerState.movingState;

    private Transform oldPosition;
    private Transform oldParent;

    void Start () {
        movementScript = GetComponent<PlayerMovement>();
        turretControlsScript = GetComponent<PlayerTurretControls>();
        turretManagerScript = GetComponent<TurretManager>();
    }

    void Update()
    {
        if (GameController.currentFocusLayer == GameController.FocusLayer.Game)
        {
            switch (currentState)
            {
                case PlayerState.movingState:
                    if ( Input.GetKeyDown(KeyCode.T))
                    {
                        SwitchToTurret();
                    } else
                    {
                        movementScript.Move();
                    }
                    break;
                case PlayerState.turretControlState:
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        SwitchFromTurret();
                    }
                    break;
                default:
                    Debug.Log("Player state error, check player controller");
                    break;
            }
        }
    }

    private void SwitchToTurret()
    {
        associatedTurret = TurretManager.barbarianTurretActive; //TODO i mean, its obvious
        if (associatedTurret != null)
        {
            movementScript.StopMoving();

            Transform newTrans = associatedTurret.transform.GetChild(0).Find("RotationPoint").Find("PlayerPosition"); //Playerposition should be a child object of the turret
            if (newTrans != null)
            {
                oldPosition = transform; //TODO repair does not work
                transform.position = newTrans.position;
                oldParent = transform.parent;
                transform.parent = associatedTurret.transform.GetChild(0).Find("RotationPoint");
                transform.localRotation = Quaternion.identity; //zero rotation under rotationpoint
                currentState = PlayerState.turretControlState;
            }
            else
            {
                Debug.LogError("cant get player position object from turret");
            }
        }
        else
        {
            Debug.Log("Turret plz! (I mean, press T key)");
        }
    }
    private void SwitchFromTurret() {
        transform.parent = oldParent;
        transform.position = oldPosition.position;
        transform.rotation = oldPosition.rotation;
        currentState = PlayerState.movingState;

        movementScript.StartMoving();
    }
}
