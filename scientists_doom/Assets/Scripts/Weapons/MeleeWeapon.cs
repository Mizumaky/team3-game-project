using UnityEngine;

[RequireComponent (typeof (Animator))]
public class MeleeWeapon : Weapon {
  [SerializeField] private Animator animator;
  [SerializeField] private new Collider collider;
  [SerializeField] private bool enableCollision = false;

  private void Awake () {
    Init ();
  }

  private void Update () {
    GetInput ();
  }

  protected override void Init () {
    base.Init ();
    animator = GetComponent<Animator> ();
    collider = animator.GetBoneTransform (HumanBodyBones.LeftHand).GetComponentInChildren<Collider> ();
  }

  protected override void PerformBasicAttack () {
    animator.SetTrigger ("attackTrigger");
  }

  private void OnTriggerEnter (Collider other) {
    int otherLayer = other.gameObject.layer;

    int enemyLayer = LayerMask.NameToLayer (isPlayerControlled ? "Player" : "Enemy");

    if (enableCollision == true) {
      if (otherLayer == enemyLayer) {
        other.GetComponent<EnemyControls> ().Aggro (transform);
        other.GetComponent<Stats> ().TakeDamage (GetCurrentTotalDamage ());
      }
    }
  }

  // Changed by avatar attack animation
  public void ToggleCollision () {
    enableCollision = !enableCollision;
  }

}