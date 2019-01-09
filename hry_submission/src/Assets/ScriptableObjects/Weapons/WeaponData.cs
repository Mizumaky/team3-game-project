using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponData/Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
  [Header("General")]
  public string weaponName = "Weapon";
  public Sprite icon;
  public string description = "This is a weapon!";
  public int levelReq;
  public int upgradeCost = 0;

  [Header("Damage")]
  public int damage;
}