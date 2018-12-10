using UnityEngine;

public class AbilityManager : MonoBehaviour
{
  public enum AbilityTypes { Passive, Basic, First, Second }

  [Range(1, 3)]
  public int[] abilityRanks = { 1 };
  public Ability[] abilities = new Ability[4];

  public void IncreaseAbilityRank(AbilityTypes type)
  {
    if (abilityRanks[(int)type] < 3)
    {
      abilityRanks[(int)type]++;
      abilities[(int)type].IncreaseRank();
    }
  }

}