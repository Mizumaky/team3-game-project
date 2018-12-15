using UnityEngine;

[CreateAssetMenu(fileName = "AbilityRankData", menuName = "AbilityRankData/Unspecified", order = 0)]
public class AbilityRankData : ScriptableObject
{
  public string abilityName = "New Ability";
  public Sprite icon;
  public Ability.Rank rank = Ability.Rank.Basic;
}