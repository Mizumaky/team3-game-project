using UnityEngine;

[CreateAssetMenu(fileName = "Rage", menuName = "AbilityRankData/Barbarian/Rage", order = 0)]
public class BarbRageRankData : AbilityRankData
{
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;
  public GameObject ragePSPrefab;
}