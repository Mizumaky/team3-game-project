using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BarbarianRagePassiveAbility))]
public class BarbarianKegThrowAbility : Ability
{
  [Header("Key")]
  public KeyCode keyCode = KeyCode.Q;

  [Header("Parameters From Ability Data")]
  public int damageOnSplash;
  public float spillDecayTime;
  public float timeToLive;
  public GameObject kegPrefab;
  public GameObject spillPrefab;

  [Header("Parameters")]
  public float noFlightDistance;
  public AnimationCurve travelHeightPercentage;
  public Transform spawnPointTransform;

  private void Update()
  {
    if (Input.GetKeyDown(keyCode))
    {
      Throw();
    }
  }

  private void Throw()
  {
    GameObject bobek = Instantiate(kegPrefab, spawnPointTransform.position, kegPrefab.transform.rotation, null);
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is KegThrowRankData)
    {
      KegThrowRankData data = ((KegThrowRankData)abilityRankData[(int)rank]);
      damageOnSplash = data.damageOnSplash;
      spillDecayTime = data.spillDecayTime;
      timeToLive = data.timeToLive;
      kegPrefab = data.kegPrefab;
      spillPrefab = data.spillPrefab;
    }
    else
    {
      Debug.LogWarning("BarbarianKegThrowAbility: Invalid ability data!");
    }
  }
}