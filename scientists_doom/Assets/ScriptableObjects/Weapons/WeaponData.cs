using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "WeaponData/Weapon", order = 0)]
public class WeaponData : ScriptableObject {
  public string name = "Weapon";
  public Sprite icon;
  public string description = "This is a weapon!";
  public int upgradeCost = 0;
}