using UnityEngine;

public class PlayerAttacksBarbarian : MonoBehaviour {

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            Fire();
        }
	}

    void Fire() {
        animator.SetTrigger("attackTrigger");
    }
}
