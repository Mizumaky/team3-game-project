using UnityEngine;

[CreateAssetMenu(fileName = "Keg Throw", menuName = "AbilityRankData/Barbarian/Keg Throw", order = 0)]
public class KegThrowRankData : AbilityRankData
{
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;
  public GameObject ragePSPrefab;
}