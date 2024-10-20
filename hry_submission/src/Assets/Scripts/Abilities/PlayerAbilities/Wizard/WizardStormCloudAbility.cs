using UnityEngine;

[RequireComponent(typeof(Stats))]
public class WizardStormCloudAbility : WizardChargedAbility
{
  #region Variables
  [Header("HIT")]
  public float radius;

  [Header("Object")]
  public float cloudVelocity;
  public float cloudDestinationHeight;

  [Header("Visuals")]
  public GameObject areaOutlinePrefab;

  [Header("Scriptable Parameters")]
  private int _damagePerTick;
  public int damagePerTick { get { return _damagePerTick; } }
  private float _duration;
  public float duration { get { return _duration; } }

  #endregion

  public override void SetAndRelease()
  {
    base.SetAndRelease();
    StormCloudProjectile proj = chargePassive.chargedObject.GetComponent<StormCloudProjectile>();
    if (proj == null)
    {
      Debug.LogWarning("Invalid projectile prefab attached!");
      return;
    }

    float totalDamagePerTick = (GetComponent<Stats>().GetAttackDamage() / (duration / 0.5f) + damagePerTick) * chargePassive.chargeFactor;
    float finalRadius = radius * chargePassive.chargeFactor;

    Vector3 groundPosAtMouse = PlayerMovement.GetGroundPosAtMouse();
    Vector3 destination = groundPosAtMouse + Vector3.up * cloudDestinationHeight;

    proj.SetAndRelease(destination, groundPosAtMouse, totalDamagePerTick, finalRadius, cloudVelocity, duration, transform, areaOutlinePrefab);
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is StormCloudRankData)
    {
      StormCloudRankData data = ((StormCloudRankData)abilityRankData[(int)rank]);
      _damagePerTick = data.damagePerTick;
      _duration = data.duration;
    }
    else
    {
      Debug.LogWarning("WizardStormCloudAbility: Invalid ability data!");
    }
  }
}