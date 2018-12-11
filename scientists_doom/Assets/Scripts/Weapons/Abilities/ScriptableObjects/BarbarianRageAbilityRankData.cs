using UnityEngine;

[CreateAssetMenu(fileName = "BarbarianRageAbility", menuName = "AbilityRankData/Barbarian", order = 0)]
public class BarbarianRageAbilityRankData : AbilityRankData
{
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;
  public float stackDecayPeriod;
  public GameObject ragePSPrefab;
}