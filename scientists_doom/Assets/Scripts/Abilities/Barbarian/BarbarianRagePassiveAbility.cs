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
  public bool canStack = true;
  public float stackDecayPeriod = 1f;

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
    stackDecayStartDelayWFS = new WaitForSeconds(stackDecayPeriod);
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is BarbRageRankData)
    {
      BarbRageRankData data = ((BarbRageRankData)abilityRankData[(int)rank]);
      perStackDamageIncrement = data.perStackDamageIncrement;
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