using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour {

    public float cooldown;
    private bool readyToAttack;
    private bool attacking;
    private float damage;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        readyToAttack = true;
        attacking = false;
        damage = gameObject.GetComponent<EnemyStats>().enemyDamage;
	}

    public void StartAttackPlayer(GameObject player)
    {
        attacking = true;
        StartCoroutine(AttackPlayer(player));
    }

    public void StopAttackPlayer()
    {
        attacking = false;
    }

    IEnumerator AttackPlayer(GameObject player)
    {
        while (attacking && player != null) {
            animator.SetTrigger("attackTrigger");
            player.GetComponent<PlayerStats>().TakeDamage(damage);
            yield return new WaitForSeconds(cooldown);
        }
        attacking = false;
    }
}
