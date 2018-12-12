using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BarbarianRagePassiveAbility))]
public class BarbarianImmortalityAbility : Ability
{
  #region Variables
  [Header("Key")]
  public KeyCode keyCode = KeyCode.E;

  [Header("Parameters From Ability Data")]
  public int stackRequirement;
  public float duration;

  [Header("Parameters")]
  public bool isAvailable = false;
  public float updatePeriodFloat = 1f;

  private Coroutine activeImmortalityRoutine;
  private WaitForSeconds updatePeriod;
  private BarbarianRagePassiveAbility barbarianRageAbility;
  #endregion

  private void Init()
  {
    barbarianRageAbility = GetComponent<BarbarianRagePassiveAbility>();
    updatePeriod = new WaitForSeconds(updatePeriodFloat);
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode))
    {
      if (isAvailable)
      {
        Cast();
      }
      else
      {
        // TODO: Display a message that spell is not available (make a static class for messages like theese)
      }
    }
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is BarbImmortalityRankData)
    {
      BarbImmortalityRankData data = ((BarbImmortalityRankData)abilityRankData[(int)rank]);
      stackRequirement = data.stackRequirement;
      duration = data.duration;
    }
    else
    {
      Debug.LogWarning("BarbarianImmortalityAbility: Invalid ability data!");
    }
  }

  private void UpdateAvailability()
  {
    if (barbarianRageAbility.stacks >= stackRequirement)
    {
      isAvailable = true;
    }
  }

  private IEnumerator Immortality()
  {
    float durLeft = duration;
    Debug.Log("Immortal!");

    barbarianRageAbility.canStack = false;
    // TODO: Prevent taking damage (probably somewhere in stats)

    while (durLeft > 0)
    {
      durLeft -= updatePeriodFloat;
      // TODO: Update some sort of a duration indicator
      yield return updatePeriod;
    }

    barbarianRageAbility.canStack = true;
    // TODO: Revert
    Debug.Log("Not immortal!");
  }

  public void Cast()
  {
    barbarianRageAbility.ResetStacks();
    activeImmortalityRoutine = StartCoroutine(Immortality());
  }

  // TODO: Add a listener for barb rage update
}