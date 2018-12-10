using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BarbarianRagePassiveAbility : MonoBehaviour
{
  #region Variables
  [Header("Refs")]
  public ParticleSystem ps;
  public BarbarianRageAbilityData data; // TODO: Set data via ability manager

  [Header("Parameters")]
  public int stacks = 0;
  public bool canStack = true;

  private Coroutine activeStackDecayRoutine;
  private WaitForSeconds stackDecayStartDelay;
  private WaitForSeconds stackDecayPeriod;
  private Stats stats;
  #endregion

  private void Awake()
  {
    Init();
  }

  private void Init()
  {
    stats = GetComponent<Stats>();

    stackDecayStartDelay = new WaitForSeconds(data.rank.stackDecayStartDelay);
    stackDecayPeriod = new WaitForSeconds(data.rank.stackDecayPeriod);
  }

  public void UpdateRank(int n) { }

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
    if (ps != null)
    {
      // TODO: Update ps
    }
    else
    {
      Debug.LogWarning("BarbarianRageAbility: Error - No particle system set!");
    }
  }
}