using UnityEngine;

[CreateAssetMenu (fileName = "AbilityRankData", menuName = "AbilityRankData/Unspecified", order = 0)]
public class AbilityRankData : ScriptableObject {
  [Header ("General")]
  public string abilityName = "New Ability";
  public Sprite icon;
  public string description;
  public int upgradeCost;

  [Header ("Rank")]
  public Ability.Rank rank = Ability.Rank.Basic;

  [Header ("Cooldown")]
  public float cooldown;

}