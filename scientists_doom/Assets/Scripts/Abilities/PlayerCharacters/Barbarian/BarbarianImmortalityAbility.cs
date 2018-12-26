using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BarbarianImmortalityAbility : Ability
{
  #region Variables
  public ParticleSystem rageParticleSystem;

  [Header("Parameters From Ability Data")]
  public int stackCost;
  public float duration;
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;


  [Header("Parameters")]
  public bool isAvailable = true;
  public float updatePeriodFloat = 1f;

  public SkinnedMeshRenderer modelRenderer;

  private bool isImmortal;
  private Coroutine activeImmortalityRoutine;
  private WaitForSeconds updatePeriod;

  [Header("Rage Passive")]
  public int stacks = 0;
  public int stacksCap = 100;
  public bool canStack = true;
  public int stackDecrement;
  public float stackDecayPeriod = 1f;

  private Coroutine activeStackDecayRoutine;
  private WaitForSeconds stackDecayStartDelayWFS;
  private WaitForSeconds stackDecayPeriodWFS;
  private Stats stats;
  #endregion

  private void Awake()
  {
    Init();
  }

  private void Start()
  {
    isImmortal = false;
    ResetStacks();
    stackDecayPeriodWFS = new WaitForSeconds(stackDecayPeriod);
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode) && !onCooldown)
    {
      if (isAvailable && !isImmortal)
      {
        Cast();
      }
      else
      {
        Debug.LogWarning("BarbImmortalityAbility: Not available!");
      }
    }
  }

  private void Init()
  {
    stats = GetComponent<Stats>();
    updatePeriod = new WaitForSeconds(updatePeriodFloat);
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is BarbImmortalityRankData)
    {
      BarbImmortalityRankData data = ((BarbImmortalityRankData)abilityRankData[(int)rank]);
      stackCost = data.stackCost;
      duration = data.duration;
      stacksCap = data.stacksCap;
      perStackDamageIncrement = data.perStackDamageIncrement;
      stackDecrement = data.stackDecrement;
      stackDecayStartDelay = data.stackDecayStartDelay;

      stackDecayStartDelayWFS = new WaitForSeconds(stackDecayStartDelay);
    }
    else
    {
      Debug.LogWarning("BarbarianImmortalityAbility: Invalid ability data!");
    }
  }

  private void UpdateAvailability()
  {
    if (stacks >= stackCost)
    {
      isAvailable = true;
    }
    else
    {
      isAvailable = false;
    }
  }

  private IEnumerator Immortality()
  {
    float durLeft = duration;
    isImmortal = true;
    canStack = false;

    Color temp = modelRenderer.materials[0].color;
    modelRenderer.materials[0].color = Color.black;

    stats.isInvulnerable = true;

    while (durLeft > 0)
    {
      durLeft -= updatePeriodFloat;
      // TODO: Update some sort of a duration indicator
      yield return updatePeriod;
    }

    canStack = true;
    modelRenderer.materials[0].color = temp;
    stats.isInvulnerable = false;

    isImmortal = false;
  }

  public void Cast()
  {
    UseStacks(stackCost);
    UpdateAvailability();
    activeImmortalityRoutine = StartCoroutine(Immortality());

    StartCoroutine(CooldownRoutine());
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
        UpdateAvailability();
      }
    }
    else
    {
      Debug.LogWarning("BarbarianRageAbility: Cannot stack right now!");
    }
  }

  public void UseStacks(int amount)
  {
    stacks -= amount;
    stats.RemoveBonusDamage(amount * perStackDamageIncrement);
    UpdatePS();
    UpdateHUD();
    UpdateAvailability();
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

  public void ResetStacks()
  {
    stacks = 0;
    stats.ResetBonusDamage();
    UpdatePS();
    UpdateHUD();
    EventManager.TriggerEvent("updateImmortalityAv");
  }
}