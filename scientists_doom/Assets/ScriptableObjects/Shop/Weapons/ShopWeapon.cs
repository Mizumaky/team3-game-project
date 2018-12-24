using UnityEngine;

[CreateAssetMenu(fileName = "ShopWeapon", menuName = "Shop/ShopWeapon", order = 0)]
public class ShopWeapon : ScriptableObject {
	public string weaponName;
	public Sprite icon;
	public string description;
	public int price = 999;
}
