using UnityEngine;

[CreateAssetMenu(fileName = "Immortality", menuName = "AbilityRankData/Barbarian/Immortality", order = 0)]
public class BarbImmortalityRankData : AbilityRankData
{
  public int stackCost = 100;
  public float duration = 5f;
  public int stacksCap;
  public int perStackDamageIncrement;
  public int stackDecrement;
  public float stackDecayStartDelay;
}
