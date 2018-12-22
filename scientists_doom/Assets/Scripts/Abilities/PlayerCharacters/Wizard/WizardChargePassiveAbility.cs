using System.Collections;
using UnityEngine;

public class WizardChargePassiveAbility : Ability
{
  public Transform chargedObjectSpawnTransform;
  public GameObject chargedObject;

  public float chargeProgress = 0;
  public float chargeFactor;
  public bool hasReleased = true;

  private WizardChargedAbility ability;

  public void BeginCharging(WizardChargedAbility ability)
  {
    this.ability = ability;

    if (ability.chargedObjectPrefab == null)
    {
      Debug.LogWarning("No charged object prefab set!");
      return;
    }

    hasReleased = false;
    chargedObject = Instantiate(ability.chargedObjectPrefab, chargedObjectSpawnTransform.position, transform.rotation, chargedObjectSpawnTransform) as GameObject;
    StartCoroutine(ChargeUpdateRoutine());
  }

  private IEnumerator ChargeUpdateRoutine()
  {
    while (chargeProgress < 1 && !hasReleased)
    {
      UpdateCharge();

      yield return null;
    }
  }

  private void UpdateCharge()
  {
    chargeProgress += Time.deltaTime;
    chargeFactor = 1 + chargeProgress * Mathf.Pow(2, ability.maxCharge);
    chargedObject.transform.localScale = chargeFactor * Vector3.one;

    EventManager.TriggerEvent("updateCharSpec");
  }

  public virtual void Release(WizardChargedAbility ability)
  {
    if (this.ability != ability)
    {
      return;
    }

    hasReleased = true;

    if (chargeProgress < ability.minChargeForRelease)
    {
      Interrupt();
    }
    else
    {
      chargedObject.transform.parent = null;
      ability.SetAndRelease();
    }
    Reset();
  }

  private void Interrupt()
  {
    Destroy(chargedObject);
  }

  private void Reset()
  {
    chargeProgress = 0;
    chargeFactor = 0;
    chargedObject = null;

    EventManager.TriggerEvent("updateCharSpec");
  }
}