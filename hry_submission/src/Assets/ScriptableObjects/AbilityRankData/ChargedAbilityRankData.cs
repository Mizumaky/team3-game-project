using UnityEngine;

[CreateAssetMenu (fileName = "Charged Ability Rank Data", menuName = "Ability Rank Data/Charged Ability", order = 0)]
public class ChargedAbilityRankData : AbilityRankData {
  [Header ("Charge")]
  public float maxCharge;
}