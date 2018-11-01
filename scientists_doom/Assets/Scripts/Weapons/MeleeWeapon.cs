using UnityEngine;

[RequireComponent (typeof (Animator), typeof (ColliderBridge))]
public class MeleeWeapon : Weapon {
  public enum WeaponHand { Right, Left }
  public WeaponHand weaponHand;
  [SerializeField] private Animator animator;
  [SerializeField] private new Collider collider;
  [SerializeField] private bool enableCollision = false;

  private void Awake () {
    Init ();
  }

  private void Start () {
    // Get weapon collider from the correct hand
    if (weaponHand == WeaponHand.Left) {
      collider = animator.GetBoneTransform (HumanBodyBones.LeftHand).GetComponentInChildren<Collider> ();
    } else {
      collider = animator.GetBoneTransform (HumanBodyBones.RightHand).GetComponentInChildren<Collider> ();
    }
  }

  private void Update () {
    if (isPlayerControlled) {
      GetInput ();
    }
  }

  protected override void Init () {
    base.Init ();
    animator = GetComponent<Animator> ();
    ColliderBridge colliderBridge = GetComponent<ColliderBridge> ();
    colliderBridge.Initialize (this);
  }

  protected override void PerformBasicAttack () {
    animator.SetTrigger ("attackTrigger");
  }

  public void OnWeaponTriggerEnter (Collider other) {
    if (enableCollision == true) {
      int otherLayer = other.gameObject.layer;

      // Target is enemy
      if (isPlayerControlled) {
        if (otherLayer == LayerMask.NameToLayer ("Enemy")) {
          other.GetComponent<EnemyControls> ().Aggro (transform);
        }
      }

      // Any target
      int targetLayer = LayerMask.NameToLayer (isPlayerControlled ? "Enemy" : "Player");
      if (otherLayer == targetLayer) {
        other.GetComponent<Stats> ().TakeDamage (GetCurrentTotalDamage ());
      }
    }
  }

  // Changed by avatar attack animation
  public void ToggleCollision () {
    enableCollision = !enableCollision;
  }

}