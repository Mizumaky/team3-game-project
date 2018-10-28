using System.Collections;
using UnityEngine;

//TODO wizard and barbarian have different classes for attack,
//would like to make it work similiarily as for movement

public class PlayerController : MonoBehaviour {

    public GameObject associatedTurret;
 //   public Transform playerPositionAtTurret;
    protected PlayerMovement movementScript;
    protected PlayerTurretControls turretScript;

    protected enum PlayerState
    {
        movingState,
        turretControlState
    }
    protected PlayerState currentState = PlayerState.movingState;

    void Start () {
        movementScript = GetComponent<PlayerMovement>();
        turretScript = GetComponent<PlayerTurretControls>();
    }

    void Update()
    {
        if (GameController.currentFocusLayer == GameController.FocusLayer.Game)
        {
            switch (currentState)
            {
                case PlayerState.movingState:
                    if (Input.GetKeyDown(KeyCode.R))
                    {

                        transform.position = associatedTurret.gameObject.GetComponentInChildren<Transform>().Find("PlayerPosition").position; //Playerposition should be a child object of the turret
                        //transform.rotation = transform.parent.Find("PlayerPosition").rotation;
                        transform.parent = associatedTurret.transform; //attach player to the turret
                        currentState = PlayerState.turretControlState;
                       
                    }
                    movementScript.Move();
                    break;
                case PlayerState.turretControlState:
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        currentState = PlayerState.movingState;
                    }
                    break;
                default:
                    Debug.Log("Player state error, check player controller");
                    break;
            }
        }
    }
}
