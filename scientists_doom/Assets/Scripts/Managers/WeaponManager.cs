using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
	public GameObject[] weapons;

	private GameObject activeWeapon;
	private Transform hand;

	public Vector3 attachPosition;

	void Awake(){
		hand = transform.FindDeepChild("Hand_L");
		if(hand){
			EquipWeapon(2);
		}else{
			Debug.Log("Could not find the characters hand!!!");
		}
	}
	///<summary>
	///Equip weapon by tier (0 - starting, 1 - advanced, 2 - master)
	///</summary>
	public void EquipWeapon(int weaponIndex){
		Debug.Log("Spawning barbarian weapon("+ weapons[weaponIndex].name +")");
		activeWeapon = Instantiate(weapons[weaponIndex]);

		activeWeapon.transform.parent = hand;
		activeWeapon.transform.localPosition = attachPosition;
		activeWeapon.transform.localEulerAngles = new Vector3(0,0,0);
	}

	public void GetActiveWeapon(){

	}
}
