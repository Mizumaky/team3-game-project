using UnityEngine;

public class Ability : MonoBehaviour
{
  public AbilityData[] abilityData;
  public AbilityData currentRankAbilityData;
  [Range(1, 3)]
  public int rank;

  public virtual void IncreaseRank()
  {
    if (rank < 3)
    {
      rank++;
    }
    else
    {
      Debug.LogWarning("Ability: Cannot upgrade " + currentRankAbilityData.name + " anymore!");
    }
  }

  public virtual void UpdateAbilityData()
  {
    currentRankAbilityData = abilityData[rank - 1];
  }
}