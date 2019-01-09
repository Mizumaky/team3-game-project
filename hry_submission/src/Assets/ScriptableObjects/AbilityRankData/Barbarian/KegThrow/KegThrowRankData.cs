using UnityEngine;

[CreateAssetMenu (fileName = "Keg Throw", menuName = "AbilityRankData/Barbarian/Keg Throw", order = 0)]
public class KegThrowRankData : AbilityRankData {
  [Header ("Keg Throw")]
  public int damage;
  public float spillDuration;
}