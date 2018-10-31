using UnityEngine;

public class PlayerAttacksBarbarian : MonoBehaviour {

  public Collider axeCollider;

  private Animator animator;
  private PlayerStateController controllerScript;

  private void Start () {
    axeCollider = GetComponentInChildren<BoxCollider> ();
    axeCollider.enabled = false;

    animator = GetComponentInChildren<Animator> ();
    controllerScript = GetComponent<PlayerStateController> ();
  }

  void Update () {

    if (controllerScript.currentState == PlayerStateController.PlayerState.movingState && Input.GetKeyDown (KeyCode.Space)) {
      Fire ();
    }
  }

  void Fire () {

    animator.SetTrigger ("attackTrigger");

  }

  public void AxeSwingStart () {
    axeCollider.enabled = true;
  }

  public void AxeSwingEnd () {
    axeCollider.enabled = false;
  }
}