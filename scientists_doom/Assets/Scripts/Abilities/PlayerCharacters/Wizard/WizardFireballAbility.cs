using UnityEngine;

[RequireComponent(typeof(Stats))]
public class WizardFireballAbility : WizardChargedAbility
{
  #region Variables

  [Header("Collision")]
  public LayerMask collisionMask;

  [Header("Travel")]
  public float velocityMagnitude;
  public float travelHeight = 0.5f;
  public float timeToLive = 2;

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }

  #endregion

  public override void SetAndRelease()
  {
    FireballProjectile proj = chargePassive.chargedObject.GetComponent<FireballProjectile>();
    if (proj == null)
    {
      Debug.LogWarning("Invalid projectile prefab attached!");
      return;
    }

    float totalDamage = (GetComponent<Stats>().GetAttackDamage() + damage) * chargePassive.chargeFactor;
    float finalVelocityMagnitude = velocityMagnitude * chargePassive.chargeFactor;

    chargePassive.chargedObject.GetComponent<Rigidbody>().velocity = transform.forward * finalVelocityMagnitude;
    proj.Set(totalDamage, transform, travelHeight, timeToLive, collisionMask);
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is FireballRankData)
    {
      FireballRankData data = ((FireballRankData)abilityRankData[(int)rank]);
      _damage = data.damage;
    }
    else
    {
      Debug.LogWarning("HuntressQuickShotAbility: Invalid ability data!");
    }
  }
}