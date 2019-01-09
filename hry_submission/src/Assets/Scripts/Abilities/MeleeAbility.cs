using UnityEngine;

[RequireComponent(typeof(ColliderBridge), typeof(Animator))]
public class MeleeAbility : Ability
{
  public enum WeaponHand { Right, Left }

  [Header("Parameters")]
  public WeaponHand weaponHand;
  public bool enableCollision = false;

  protected Collider col;
  protected Animator anim;

  private void Awake()
  {
    Init();
  }

  private void Start()
  {
    HumanBodyBones handId = (weaponHand == WeaponHand.Left ? HumanBodyBones.LeftHand : HumanBodyBones.RightHand);
    col = anim.GetBoneTransform(handId).GetComponentInChildren<Collider>();
  }

  protected virtual void Init()
  {
    anim = GetComponent<Animator>();

    // Bridge weapon collider events onto this object's collider
    ColliderBridge cb = GetComponent<ColliderBridge>();
    cb.Initialize(this);
  }

  public void OnWeaponTriggerEnter(Collider other)
  {
    Collide(other);
  }

  protected virtual void Collide(Collider other)
  {

  }

  public void ToggleCollision()
  {
    enableCollision = !enableCollision;
  }
}