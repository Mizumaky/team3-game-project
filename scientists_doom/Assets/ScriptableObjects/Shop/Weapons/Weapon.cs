using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 0)]
public class Weapon : ScriptableObject {
	public string weaponName;
	public Sprite icon;
	public string description;
	public int price = 999;
}
