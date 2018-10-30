using UnityEngine.AI;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public float enemyMaxHealth = 50;
    public float enemyHealth;
    private Animator animator;
    public bool enemyAlive = true;
    public float enemyDamage = 5;

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
        GetComponent<EnemyHealthBar>().AdjustHealthBar(enemyHealth / enemyMaxHealth);
    }

    public void KillEnemy()
    {
        if (animator != null)
        {
            animator.SetTrigger("dieTrigger");

            enemyAlive = false;

            GetComponent<EnemyControls>().DisableCollision();
            GetComponent<EnemyControls>().DisableMovement();
            Destroy(gameObject, 3f);
        }
        else {
            Debug.Log("Enemy does not have animator component!");
            Destroy(gameObject);
        }
    }
}
