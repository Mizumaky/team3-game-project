using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BarbarianRageAbility))]
public class BarbarianImmortalityAbility : MonoBehaviour
{
  #region Variables
  [Header("Key")]
  public KeyCode keyCode = KeyCode.E;

  [Header("Parameters")]
  public int stackRequirement = 100;
  public float duration = 5f;
  public float updatePeriodFloat = 1f;
  public bool isAvailable = false;

  private Coroutine activeImmortalityRoutine;
  private WaitForSeconds updatePeriod;
  private BarbarianRageAbility barbarianRageAbility;
  #endregion

  private void Init()
  {
    updatePeriod = new WaitForSeconds(updatePeriodFloat);
    barbarianRageAbility = GetComponent<BarbarianRageAbility>();
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
  }

  public void Cast()
  {
    barbarianRageAbility.ResetStacks();
    activeImmortalityRoutine = StartCoroutine(Immortality());
  }

  // TODO: Add a listener for barb rage update
}