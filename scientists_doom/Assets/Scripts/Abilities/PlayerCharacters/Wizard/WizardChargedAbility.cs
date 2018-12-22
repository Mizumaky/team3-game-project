using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WizardChargePassiveAbility))]
public abstract class WizardChargedAbility : Ability
{
  #region Variables
  public float minChargeForRelease;
  public GameObject chargedObjectPrefab;
  protected WizardChargePassiveAbility chargePassive;

  [Header("Scriptable Parameters")]
  protected float _maxCharge;
  public float maxCharge { get { return _maxCharge; } }

  #endregion

  private void Start()
  {
    chargePassive = GetComponent<WizardChargePassiveAbility>();
  }

  private void Update()
  {
    if (chargePassive.hasReleased)
    {
      if (Input.GetKeyDown(keyCode))
      {
        chargePassive.BeginCharging(this);
      }
    }
    else
    {
      if (Input.GetKeyUp(keyCode))
      {
        chargePassive.Release(this);
      }
    }
  }

  public virtual void SetAndRelease()
  {
    Debug.Log("Releasing " + abilityName + "!");
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is ChargedAbilityRankData)
    {
      ChargedAbilityRankData data = ((ChargedAbilityRankData)abilityRankData[(int)rank]);
      _maxCharge = data.maxCharge;
    }
    else
    {
      Debug.LogWarning("ChargedAbility: Invalid ability data!");
    }
  }
}