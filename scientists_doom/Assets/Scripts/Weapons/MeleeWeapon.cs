using UnityEngine;

[RequireComponent(typeof(Animator), typeof(WeaponColliderBridge))]
public class MeleeWeapon : Weapon
{
  public enum WeaponHand { Right, Left }
  public WeaponHand weaponHand;
  [SerializeField] private Animator animator;
  [SerializeField] private new Collider collider;
  [SerializeField] private bool enableCollision = false;

  private void Awake()
  {
    Init();
  }

  private void Start()
  {
    // Get weapon collider from the correct hand
    HumanBodyBones handId = (weaponHand == WeaponHand.Left ? HumanBodyBones.LeftHand : HumanBodyBones.RightHand);
    collider = animator.GetBoneTransform(handId).GetComponentInChildren<Collider>();
  }

  private void Update()
  {
    if (isPlayerControlled)
    {
      GetInput();
    }
  }

  protected override void Init()
  {
    base.Init();
    animator = GetComponent<Animator>();

    // Bridge weapon collider's litener to this object
    WeaponColliderBridge colliderBridge = GetComponent<WeaponColliderBridge>();
    colliderBridge.Initialize(this);
  }

  public void OnWeaponTriggerEnter(Collider other)
  {
    if (enableCollision == true)
    {
      int otherLayer = other.gameObject.layer;

      // Target is enemy
      if (isPlayerControlled)
      {
        if (otherLayer == LayerMask.NameToLayer("Enemy"))
        {
          other.GetComponent<EnemyControls>().Aggro(transform);
        }

        if (otherLayer == LayerMask.NameToLayer("Explosive"))
        {
          other.GetComponent<Explosive>().Explode(transform);
        }
      }

      // Any target
      int targetLayer = LayerMask.NameToLayer(isPlayerControlled ? "Enemy" : "Player");
      if (isPlayerControlled)
      {
        if (otherLayer == LayerMask.NameToLayer("Enemy"))
        {
          other.GetComponent<Stats>().TakeDamage(GetCurrentStatPlusWeaponDamage() + regularAttack.GetDamage());
        }
      }
      else
      {
        if (otherLayer == LayerMask.NameToLayer("Player") || otherLayer == LayerMask.NameToLayer("Castle"))
        {
          EventManager.TriggerEvent("updateHUD");
          other.GetComponent<Stats>().TakeDamage(GetCurrentStatPlusWeaponDamage() + regularAttack.GetDamage());
        }
      }
    }
  }

  // Changed by avatar attack animation
  public void ToggleCollision()
  {
    enableCollision = !enableCollision;
  }

}