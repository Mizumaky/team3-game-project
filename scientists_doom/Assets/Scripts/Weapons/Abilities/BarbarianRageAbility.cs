using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BarbarianRageAbility : MonoBehaviour
{
  #region Variables
  [Header("Refs")]
  public ParticleSystem ps;

  [Header("Parameters")]
  public int stacks = 0;
  public int perStackDamageIncrement = 10;
  public float stackDecayStartDelayFloat = 5f;
  public float stackDecayPeriodFloat = 1f;
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

    stackDecayStartDelay = new WaitForSeconds(stackDecayStartDelayFloat);
    stackDecayPeriod = new WaitForSeconds(stackDecayPeriodFloat);
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