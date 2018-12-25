using UnityEngine;

[RequireComponent(typeof(Stats))]
public class WizardFrozenBreathAbility : WizardChargedAbility
{
  #region Variables
  [Header("Area")]
  public float radius;

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }
  private int _angle;
  public int angle { get { return _angle; } }
  private float _stunDuration;
  public float stunDuration { get { return _stunDuration; } }

  #endregion

  public override void SetAndRelease()
  {
    FreezeChargeup proj = chargePassive.chargedObject.GetComponent<FreezeChargeup>();
    if (proj == null)
    {
      Debug.LogWarning("Invalid projectile prefab attached!");
      return;
    }

    float totalDamage = (GetComponent<Stats>().GetAttackDamage() + damage) * chargePassive.chargeFactor;
    float finalRadius = radius * chargePassive.chargeFactor;

    proj.Set(totalDamage, finalRadius, angle, stunDuration, transform);
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is FreezeRankData)
    {
      FreezeRankData data = ((FreezeRankData)abilityRankData[(int)rank]);
      _damage = data.damage;
      _angle = data.angle;
      _stunDuration = data.stunDuration;
    }
    else
    {
      Debug.LogWarning("WizardFrozenBreathAbility: Invalid ability data!");
    }
  }
}