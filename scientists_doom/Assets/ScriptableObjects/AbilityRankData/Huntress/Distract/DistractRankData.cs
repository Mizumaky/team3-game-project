using UnityEngine;

[CreateAssetMenu (fileName = "Distract", menuName = "AbilityRankData/Huntress/Distract", order = 0)]
public class DistractRankData : AbilityRankData {
  [Header ("Distract")]
  public float radius;
  public int duration;
}