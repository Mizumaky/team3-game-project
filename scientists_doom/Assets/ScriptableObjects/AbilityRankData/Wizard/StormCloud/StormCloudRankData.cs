using UnityEngine;

[CreateAssetMenu (fileName = "Storm Cloud", menuName = "AbilityRankData/Wizard/Storm Cloud", order = 0)]
public class StormCloudRankData : ChargedAbilityRankData {
  [Header ("Storm Cloud")]
  public int damagePerTick;
  public int duration;
}