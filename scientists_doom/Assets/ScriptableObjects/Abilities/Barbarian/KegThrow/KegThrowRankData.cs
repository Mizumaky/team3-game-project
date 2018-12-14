using UnityEngine;

[CreateAssetMenu(fileName = "Keg Throw", menuName = "AbilityRankData/Barbarian/Keg Throw", order = 0)]
public class KegThrowRankData : AbilityRankData
{
  public int damageOnSplash;
  public float timeToLive;
  public float spillDecayTime;
  public GameObject kegPrefab;
  public GameObject spillPrefab;
}