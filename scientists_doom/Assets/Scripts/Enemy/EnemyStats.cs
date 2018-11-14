using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : Stats {
  private Animator animator;

  private void Start () {
    Init ();
  }

  protected override void Init () {
    base.Init ();
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
      Debug.Log ("Enemy does not have an animator component!");
      Destroy (gameObject);
    }
  }
}