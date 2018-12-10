using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Abilities/AbilityData", order = 0)]
public class AbilityData : ScriptableObject
{
  public string nameModifier = "Name Modifier";
  public Sprite icon;
}