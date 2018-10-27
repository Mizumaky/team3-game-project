﻿using UnityEngine.AI;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public float enemyMaxHealth = 50;
    public float enemyHealth;
    private Animator animator;
    public bool enemyAlive = true;

    void Start()
    {
        enemyHealth = enemyMaxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            KillEnemy();
        }
        
    }

    public void KillEnemy()
    {
        if (animator != null)
        {
            animator.SetTrigger("dieTrigger");

            enemyAlive = false;

            GetComponent<EnemyControls>().DisableCollision();
            GetComponent<EnemyControls>().DisableMovement();
        }
        else {
            Debug.Log("Enemy does not have animator component!");
            Destroy(gameObject);
        }
    }
}
