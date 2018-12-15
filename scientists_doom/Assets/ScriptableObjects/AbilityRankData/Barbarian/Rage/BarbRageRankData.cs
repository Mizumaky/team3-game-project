using UnityEngine;

[CreateAssetMenu(fileName = "Rage", menuName = "AbilityRankData/Barbarian/Rage", order = 0)]
public class BarbRageRankData : AbilityRankData
{
  public int stacksCap;
  public int perStackDamageIncrement;
  public int stackDecrement;
  public float stackDecayStartDelay;
}