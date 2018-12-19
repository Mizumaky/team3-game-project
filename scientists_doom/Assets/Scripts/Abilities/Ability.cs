using UnityEngine;

[RequireComponent(typeof(AbilityManager))]
public class Ability : MonoBehaviour
{
  public enum Rank { Basic, Apprentice, Master }

  protected string abilityName;
  public AbilityRankData[] abilityRankData;
  protected Rank rank = Rank.Basic;

  public virtual void SetRank(Rank newRank)
  {
    rank = newRank;
    UpdateAbilityData();
  }

  public void IncreaseRank()
  {
    rank++;
    UpdateAbilityData();
  }

  public virtual void UpdateAbilityData()
  {
    abilityName = abilityRankData[(int)rank].abilityName;
  }
}