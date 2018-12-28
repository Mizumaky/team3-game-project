using UnityEngine;

public class WeaponManager : MonoBehaviour {
	public GameObject[] weapons;
	private GameObject activeWeapon;
	private Transform hand;
	public Vector3 attachPosition;

	void Awake(){
		hand = transform.FindDeepChild("Hand_L");
		if(hand){
			EquipWeapon(0);
		}else{
			Debug.Log("Could not find the characters hand!!!");
		}
	}
	///<summary>
	///Equip weapon of tier weaponIndex(0 - starting, 1 - advanced, 2 - master)
	///</summary>
	public bool EquipWeapon(int weaponIndex){
		if(hand.childCount > 0){
			Destroy(hand.GetChild(0).gameObject);
		}
		Debug.Log("Spawning weapon("+ weapons[weaponIndex].name +")");
		activeWeapon = Instantiate(weapons[weaponIndex]);

		activeWeapon.transform.parent = hand;
		//activeWeapon.transform.localScale = Vector3.one;
		activeWeapon.transform.localPosition = attachPosition;
		activeWeapon.transform.localEulerAngles = new Vector3(0,0,0);

		return true;
	}

	public void GetActiveWeapon(){

	}
}
