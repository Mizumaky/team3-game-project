using UnityEngine;

public class AbilityManager : MonoBehaviour
{
  #region Variables
  public enum AbilityTypes { Basic, First, Second }

  public Ability[] abilities;
  public Ability.Rank[] abilityRanks;
  #endregion

  private void Start()
  {
    // TODO: loads ranks from save file
    UpdateAbilityRanks();
  }

  private void UpdateAbilityRanks()
  {
    for (int i = 0; i < abilityRanks.Length; i++)
    {
      if (abilities[i] == null)
      {
        Debug.LogWarning("Invalid or missing ability script!");
        continue;
      }
      abilities[i].SetRank(abilityRanks[i]);
    }
  }

  public bool IncreaseAbilityRank(AbilityTypes type)
  {
    if (abilityRanks[(int)type] < Ability.Rank.Master)
    {
      abilityRanks[(int)type]++;
      abilities[(int)type].IncreaseRank();

      Debug.Log("Upgraded ability " + abilities[(int)type] + " to rank " + abilityRanks[(int)type]);
      return true;
    }

    Debug.LogWarning("Ability: Cannot upgrade " + abilities[(int)type] + " anymore!");
    return false;
  }

  public int GetAbilityRank(AbilityTypes abilityType)
  {
    if ((int)abilityType > -1 && (int)abilityType < 4)
    {
      return (int)abilityRanks[(int)abilityType];
    }
    else
    {
      Debug.Log("Ability on index " + (int)abilityType + " does not exist!");
      return -1;
    }
  }
}