using UnityEngine;

[CreateAssetMenu (fileName = "Quick Shot", menuName = "AbilityRankData/Huntress/Quick Shot", order = 0)]
public class QuickShotRankData : AbilityRankData {
  [Header ("Quick Shot")]
  public int damage;
  public int damageEmpowered;
}