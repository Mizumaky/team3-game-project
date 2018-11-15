using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : Stats {
  private Animator animator;
  [Header("Drop After Death")]
  public GameObject lootPrefab;

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

      SpawnLoot();

      Destroy (gameObject, 3f);
    } else {
      Debug.Log ("Enemy does not have an animator component!");
      Destroy (gameObject);
    }
  }

  private void SpawnLoot() {
    //maybe add some empty game object that will store loot
    float OffsetY = 0.5f;
    Vector3 lootPosition = new Vector3(transform.position.x, transform.position.y + OffsetY, transform.position.z);
    GameObject loot = Instantiate(lootPrefab, lootPosition, transform.rotation*Quaternion.Euler(-90,0,0));
  }
}