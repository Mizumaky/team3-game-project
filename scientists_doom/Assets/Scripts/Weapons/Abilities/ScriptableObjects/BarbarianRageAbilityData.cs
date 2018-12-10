using UnityEngine;

[CreateAssetMenu (fileName = "BarbarianRageAbility", menuName = "/Abilities/Barbarian", order = 0)]
public class BarbarianRageAbilityData : ScriptableObject {

  public string abilityName;
  public Sprite icon;

  public Rank rank = new Rank1 ();

  public class Rank {
    public int perStackDamageIncrement;
    public float stackDecayStartDelay;
    public float stackDecayPeriod;
  }

  public class Rank1 : Rank {
    public new int perStackDamageIncrement = 10;
    public new float stackDecayStartDelay = 5f;
    public new float stackDecayPeriod = 1f;
  }
}