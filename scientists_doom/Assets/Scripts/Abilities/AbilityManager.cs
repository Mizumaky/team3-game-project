using UnityEngine;

public class AbilityManager : MonoBehaviour
{
  public enum AbilityTypes { Passive, Basic, First, Second }
  public Ability[] abilities;
  public Ability.Rank[] abilityRanks;

  private void Start()
  {
    // TODO: loads ranks from save file
    UpdateAbilityRanks();
  }

  private void UpdateAbilityRanks()
  {
    for (int i = 0; i < abilityRanks.Length; i++)
    {
      if (abilities[i] != null)
      {
        abilities[i].SetRank(abilityRanks[i]);
      }
      else
      {
        Debug.LogWarning("AbilityManager: Invalid ability at " + i);
      }
    }
  }

  public bool IncreaseAbilityRank(AbilityTypes type)
  {
    if (abilityRanks[(int)type] < Ability.Rank.Master)
    {
      abilityRanks[(int)type]++;
      abilities[(int)type].IncreaseRank();
      return true;
    }
    else
    {
      Debug.LogWarning("Ability: Cannot upgrade " + abilities[(int)type] + " anymore!");
      return false;
    }
  }

}