using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : Stats
{
  public override void TakeDamage(float damage)
  {
    base.TakeDamage(damage);
    GetComponent<EnemyHealthBar>().AdjustHealthBar(currentHealth / totalMaxHealth);
    GetComponent<EnemyHealthBar>().ShowDmgText(damage);
  }

  protected override void Die()
  {
    base.Die();

    GetComponent<EnemyControls>().Disable();
  }
}