using UnityEngine;

public class PlayerAttacksBarbarian : MonoBehaviour {

    public float distance = 50f;
    public float projectileVelocity = 20f;

    public GameObject projectilePrefab;
    public Transform spawnPosition;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
