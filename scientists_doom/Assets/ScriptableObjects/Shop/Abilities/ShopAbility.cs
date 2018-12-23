using UnityEngine;

[CreateAssetMenu(fileName = "ShopAbility", menuName = "Shop/ShopAbility", order = 0)]
public class ShopAbility : ScriptableObject {
	public string abilityRankName;
	public Sprite icon;
	public string description;
	public int price = 999;
}
