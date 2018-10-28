using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public float enemyMaxHealth = 50;
    public float enemyHealth;
    public float enemyDamage = 5;

    void Start()
    {
        enemyHealth = enemyMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            KillEnemy();
        }
        GetComponent<EnemyHealthBar>().AdjustHealthBar(enemyHealth/enemyMaxHealth);
    }

    public void KillEnemy()
    {
        Destroy(gameObject);
    }
}
