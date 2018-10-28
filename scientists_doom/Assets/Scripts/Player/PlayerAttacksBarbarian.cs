using UnityEngine;

public class PlayerAttacksBarbarian : PlayerController {

    private Animator animator;
    public Collider axeCollider;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        axeCollider = GetComponentInChildren<BoxCollider>();
        axeCollider.enabled = false;
    }

    void Update() {

        if (currentState == PlayerState.movingState && Input.GetKeyDown(KeyCode.Space)) {
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
