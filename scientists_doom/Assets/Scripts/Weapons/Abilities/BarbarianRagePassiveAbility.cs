using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BarbarianRagePassiveAbility : Ability
{
  [Header("Parameters From Ability Data")]
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;
  public float stackDecayPeriod;
  public ParticleSystem rageParticleSystem;

  [Header("Parameters")]
  public int stacks = 0;
  public bool canStack = true;

  private Coroutine activeStackDecayRoutine;
  private WaitForSeconds stackDecayStartDelayWFS;
  private WaitForSeconds stackDecayPeriodWFS;
  private Stats stats;

  private void Awake()
  {
    Init();
  }

  private void Init()
  {
    stats = GetComponent<Stats>();
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is BarbarianRageAbilityRankData)
    {
      BarbarianRageAbilityRankData data = ((BarbarianRageAbilityRankData)abilityRankData[(int)rank]);
      perStackDamageIncrement = data.perStackDamageIncrement;
      stackDecayStartDelay = data.stackDecayStartDelay;
      stackDecayPeriod = data.stackDecayPeriod;

      stackDecayStartDelayWFS = new WaitForSeconds(stackDecayStartDelay);
      stackDecayStartDelayWFS = new WaitForSeconds(stackDecayPeriod);
    }
    else
    {
      Debug.LogWarning("BarbarianRagePassiveAbility: Invalid ability data!");
    }
  }

  public void IncreaseStacks()
  {
    stacks++;
    activeStackDecayRoutine = StartCoroutine(StackDecayCountdown());
    UpdatePS();
  }

  public void ResetStacks()
  {
    stacks = 0;
    UpdatePS();
  }

  private IEnumerator StackDecayCountdown()
  {
    yield return stackDecayStartDelay;
    StartCoroutine(DecayStacks());
  }

  private IEnumerator DecayStacks()
  {
    while (stacks > 0)
    {
      stacks--;
      UpdatePS();
      yield return stackDecayPeriod;
    }
  }

  public void UpdatePS()
  {
    if (rageParticleSystem != null)
    {
      // TODO: Update ps
    }
    else
    {
      Debug.LogWarning("BarbarianRageAbility: Error - No particle system set!");
    }
  }
}