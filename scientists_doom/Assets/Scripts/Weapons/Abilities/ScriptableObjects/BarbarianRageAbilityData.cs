using UnityEngine;

[CreateAssetMenu(fileName = "BarbarianRageAbility", menuName = "/Abilities/Barbarian", order = 0)]
public class BarbarianRageAbilityData : AbilityData
{
  public int perStackDamageIncrement;
  public float stackDecayStartDelay;
  public float stackDecayPeriod;
}