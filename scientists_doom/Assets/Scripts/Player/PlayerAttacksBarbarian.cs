using UnityEngine;

public class PlayerAttacksBarbarian : MonoBehaviour {

    public Collider axeCollider;

    private Animator animator;
    private PlayerController controllerScript;


    private void Start()
    {
        axeCollider = GetComponentInChildren<BoxCollider>();
        axeCollider.enabled = false;

        animator = GetComponentInChildren<Animator>();
        controllerScript = GetComponent<PlayerController>();
    }

    void Update() {

        if (controllerScript.currentState == PlayerController.PlayerState.movingState && Input.GetKeyDown(KeyCode.Space)) {
            Fire();
        }
	}

    void Fire() {
        
        animator.SetTrigger("attackTrigger");

    }

    public void AxeSwingStart() {
        axeCollider.enabled = true;
    }

    public void AxeSwingEnd()
    {
        axeCollider.enabled = false;
    }
}
