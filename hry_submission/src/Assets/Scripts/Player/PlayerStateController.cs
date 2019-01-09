using System.Collections;
using UnityEngine;


//TODO wizard and barbarian have different classes for attack,
//would like to make it work similiarily as for movement
public class PlayerStateController : MonoBehaviour
{

  public enum PlayerState
  {
    movingState,
    turretControlState
  }

  [Header("In-game script references")]
  public PlayerState currentState = PlayerState.movingState;

  protected PlayerMovement movementScript;
  //protected PlayerAttacksBarbarian attacksBarbarianScript;
  protected PlayerTurretControls turretControlsScript;

  private Vector3 oldPosition;
  private Transform oldParent;

  void Start()
  {
    movementScript = GetComponent<PlayerMovement>();
    //attacksBarbarianScript = GetComponent<PlayerAttacksBarbarian>();
    turretControlsScript = GetComponent<PlayerTurretControls>();
  }

  void Update()
  {
    if (GameController.currentFocusLayer == GameController.FocusLayer.Game)
    {
      switch (currentState)
      {
        case PlayerState.movingState:
          if (Input.GetKeyDown(KeyCode.LeftShift))
          {
            SwitchToTurret();
          }
          else
          {
            movementScript.Move();
          }
          break;
        case PlayerState.turretControlState:
          if (Input.GetKeyDown(KeyCode.LeftShift))
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
    if (TurretManager.activeTurretObject != null)
    {
      movementScript.StopMoving();

      Transform newTrans = TurretManager.activeTurretObject.transform.GetChild(0).Find("RotationPoint").Find("PlayerPosition"); //Playerposition should be a child object of the turret
      if (newTrans != null)
      {
        oldPosition = transform.position;
        transform.position = newTrans.position;
        oldParent = transform.parent;
        transform.parent = TurretManager.activeTurretObject.transform.GetChild(0).Find("RotationPoint");
        transform.localRotation = Quaternion.identity; //zero rotation under rotationpoint
        currentState = PlayerState.turretControlState;

        CameraController.currentCamera = CameraController.Camera.TurretFP;
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

  private void SwitchFromTurret()
  {
    transform.parent = oldParent;
    transform.position = oldPosition;
    currentState = PlayerState.movingState;

    movementScript.StartMoving();

    CameraController.currentCamera = CameraController.Camera.PlayerDF;
  }
}