using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : Stats {
  private Animator animator;

  void Start () {
    totalMaxHealth = baseMaxHealth;
    totalAttackDamage = baseAttackDamage;
    animator = GetComponent<Animator> ();
  }

  public override void TakeDamage (float damage) {
    base.TakeDamage (damage);
    GetComponent<EnemyHealthBar> ().AdjustHealthBar (currentHealth / totalMaxHealth);
  }

  protected override void Die () {
    base.Die ();
    if (animator != null) {
      animator.SetTrigger ("dieTrigger");

      GetComponent<EnemyControls> ().DisableCollision ();
      GetComponent<EnemyControls> ().DisableMovement ();

      Destroy (gameObject, 3f);
    } else {
      Debug.Log ("Enemy does not have animator component!");
      Destroy (gameObject);
    }
  }
}