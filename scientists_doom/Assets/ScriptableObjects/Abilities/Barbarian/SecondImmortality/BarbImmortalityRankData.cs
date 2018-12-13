using UnityEngine;

[CreateAssetMenu(fileName = "Immortality", menuName = "AbilityRankData/Barbarian/Immortality", order = 0)]
public class BarbImmortalityRankData : AbilityRankData
{
  public int stackRequirement = 100;
  public float duration = 5f;
}