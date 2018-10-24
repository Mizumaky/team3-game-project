using UnityEngine;

public class PlayerAttacksBarbarian : MonoBehaviour {

    public GameObject attackSpawnPoint;
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
        attackSpawnPoint.SetActive(true);
    }
}
