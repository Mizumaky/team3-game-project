using UnityEngine;

[CreateAssetMenu (fileName = "Fireball", menuName = "AbilityRankData/Wizard/Fireball", order = 0)]
public class FireballRankData : ChargedAbilityRankData {
  [Header ("Fireball")]
  public int damage;
}