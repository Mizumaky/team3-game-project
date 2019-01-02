using UnityEngine;

[CreateAssetMenu (fileName = "Freeze", menuName = "AbilityRankData/Wizard/Freeze", order = 0)]
public class FreezeRankData : ChargedAbilityRankData {
  [Header ("Freeze")]
  public int damage;
  public int angle;
  public int stunDuration;
}