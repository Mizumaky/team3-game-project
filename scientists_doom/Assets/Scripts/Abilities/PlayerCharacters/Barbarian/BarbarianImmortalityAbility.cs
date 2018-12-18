using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BarbarianRagePassiveAbility))]
public class BarbarianImmortalityAbility : Ability
{
  #region Variables
  [Header("Key")]
  public KeyCode keyCode = KeyCode.E;

  [Header("Parameters From Ability Data")]
  public int stackCost;
  public float duration;

  [Header("Parameters")]
  public bool isAvailable = true;
  public float updatePeriodFloat = 1f;

  public SkinnedMeshRenderer modelRenderer;

  private bool isImmortal;
  private Coroutine activeImmortalityRoutine;
  private WaitForSeconds updatePeriod;
  private BarbarianRagePassiveAbility barbarianRageAbility;
  private Stats stats;
  #endregion

  private void Awake()
  {
    Init();
    EventManager.StartListening("updateImmortalityAv", UpdateAvailability);
  }

  private void Start()
  {
    isImmortal = false;
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode))
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
    barbarianRageAbility = GetComponent<BarbarianRagePassiveAbility>();
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
    }
    else
    {
      Debug.LogWarning("BarbarianImmortalityAbility: Invalid ability data!");
    }
  }

  private void UpdateAvailability()
  {
    if (barbarianRageAbility.stacks >= stackCost)
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
    barbarianRageAbility.canStack = false;

    Color temp = modelRenderer.materials[0].color;
    modelRenderer.materials[0].color = Color.black;

    stats.isInvulnerable = true;

    while (durLeft > 0)
    {
      durLeft -= updatePeriodFloat;
      // TODO: Update some sort of a duration indicator
      yield return updatePeriod;
    }

    barbarianRageAbility.canStack = true;
    modelRenderer.materials[0].color = temp;
    stats.isInvulnerable = false;

    isImmortal = false;
  }

  public void Cast()
  {
    barbarianRageAbility.UseStacks(stackCost);
    UpdateAvailability();
    activeImmortalityRoutine = StartCoroutine(Immortality());
  }
}