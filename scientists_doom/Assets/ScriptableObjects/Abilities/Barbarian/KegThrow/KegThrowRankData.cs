using UnityEngine;

[CreateAssetMenu(fileName = "Keg Throw", menuName = "AbilityRankData/Barbarian/Keg Throw", order = 0)]
public class KegThrowRankData : AbilityRankData
{
  public int dmgOnSplash;
  public float spillDecayTime;
  public GameObject kegPrefab;
  public GameObject spillPrefab;
}