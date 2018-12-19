using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BarbarianRagePassiveAbility : Ability
{
  [Header("Parameters From Ability Data")]
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;

  public ParticleSystem rageParticleSystem;

  [Header("Parameters")]
  public int stacks = 0;
  public int stacksCap = 100;
  public bool canStack = true;
  public int stackDecrement;
  public float stackDecayPeriod = 1f;

  private Coroutine activeStackDecayRoutine;
  private WaitForSeconds stackDecayStartDelayWFS;
  private WaitForSeconds stackDecayPeriodWFS;
  private Stats stats;

  private void Awake()
  {
    Init();

  }

  private void Start()
  {
    ResetStacks();
    stackDecayPeriodWFS = new WaitForSeconds(stackDecayPeriod);
  }

  private void Init()
  {
    stats = GetComponent<Stats>();
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is BarbRageRankData)
    {
      BarbRageRankData data = ((BarbRageRankData)abilityRankData[(int)rank]);
      stacksCap = data.stacksCap;
      perStackDamageIncrement = data.perStackDamageIncrement;
      stackDecrement = data.stackDecrement;
      stackDecayStartDelay = data.stackDecayStartDelay;

      stackDecayStartDelayWFS = new WaitForSeconds(stackDecayStartDelay);
    }
    else
    {
      Debug.LogWarning("BarbarianRagePassiveAbility: Invalid ability data!");
    }
  }

  public void IncreaseStacks()
  {
    if (canStack)
    {
      if (activeStackDecayRoutine != null)
      {
        StopAllCoroutines();
      }
      activeStackDecayRoutine = StartCoroutine(StackDecayCountdown());

      if (stacks < stacksCap)
      {
        stacks++;
        stats.AddBonusDamage(perStackDamageIncrement);
        UpdatePS();
        UpdateHUD();
        EventManager.TriggerEvent("updateImmortalityAv");
      }
    }
    else
    {
      Debug.LogWarning("BarbarianRageAbility: Cannot stack right now!");
    }
  }

  public void ResetStacks()
  {
    stacks = 0;
    stats.ResetBonusDamage();
    UpdatePS();
    UpdateHUD();
    EventManager.TriggerEvent("updateImmortalityAv");
  }

  public void UseStacks(int amount)
  {
    stacks -= amount;
    stats.RemoveBonusDamage(amount * perStackDamageIncrement);
    UpdatePS();
    UpdateHUD();
    EventManager.TriggerEvent("updateImmortalityAv");
  }

  private IEnumerator StackDecayCountdown()
  {
    yield return stackDecayStartDelayWFS;
    StartCoroutine(DecayStacks());
  }

  private IEnumerator DecayStacks()
  {
    while (stacks > 0)
    {
      stacks -= stackDecrement;
      stats.RemoveBonusDamage(perStackDamageIncrement * stackDecrement);
      UpdatePS();
      UpdateHUD();
      yield return stackDecayPeriodWFS;
    }
    ResetStacks();
  }

  public void UpdatePS()
  {
    if (rageParticleSystem != null)
    {
      var emission = rageParticleSystem.emission;
      emission.rateOverTime = 20f * ((float)stacks / (float)stacksCap);
    }
    else
    {
      Debug.LogWarning("BarbarianRageAbility: Error - No particle system set!");
    }
  }

  public void UpdateHUD()
  {
    EventManager.TriggerEvent("updateCharSpec");
  }

}