using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
  [HideInInspector]
  public int wood;
  [HideInInspector]
  public int stone;
  [HideInInspector]
  public int souls;

	void Start () {
    wood = 0;
    stone = 0;
    souls = 0;
  }

  void OnCollisionEnter(Collision other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Loot")){
      Loot loot = other.gameObject.GetComponent<Loot>();
      int resourcesCnt = loot.resourcesCount;
      int wd = 0;
      int stn = 0;
      if (gameObject.tag == "Barbarian") {
        //barbarian take wood:stone 1:1
        wd = resourcesCnt / 2;
        stn = resourcesCnt / 2;
      } else if (gameObject.tag == "Wizard") {
        //wizzard take wood:stone 1:3
        wd = resourcesCnt / 4;
        stn = 3 * (resourcesCnt / 4);
      } else if (gameObject.tag == "Huntress") {
        //huntress take wood:stone 3:1
        wd = 3 * (resourcesCnt / 4);
        stn = resourcesCnt / 4;
      }
      AddResources(wd, stn);
      AddSouls(1);
      loot.DestroyLootObject();
    }
  }

  public void AddResources(int _wood, int _stone) {
    wood += (int)_wood;
    stone += (int)_stone;
    Debug.Log("Adding wood " + _wood + " stone " + _stone);
  }

  public void TakeResources(int _wood, int _stone) {
    wood -= _wood;
    stone -= _stone;
  }

  public void AddSouls(int _souls) {
    souls += _souls;
  }

  public void TakeSouls(int _souls) {
    souls -= _souls;
  }

}
