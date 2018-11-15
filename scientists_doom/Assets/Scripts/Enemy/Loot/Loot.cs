using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {
  public float timeToDestroy = 0.1f;
  public int resourcesCount;
  private float lootMultiplayer;

	// Use this for initialization
	void Start () {
    lootMultiplayer = Random.Range(0.85f, 1.2f);
  }

  private void OnCollisionEnter(Collision other) {
    if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
      GiveLoot(other.gameObject.name);     
      DestroyLootObject();
    }
  }

  private void GiveLoot(string playerType) {
    int wood = 0;
    int stone = 0;
    if (playerType == "Barbarian(Clone)") {
      //barbarian take wood:stone 1:1
      wood = resourcesCount / 2;
      stone = resourcesCount / 2;
    } else if (playerType == "Wizard(Clone)") {
      //wizzard take wood:stone 1:3
      wood = resourcesCount / 4;
      stone = 3 * (resourcesCount / 4);
    } else if (playerType == "Huntress(Clone)") {
      //huntress take wood:stone 3:1
      wood = 3 * (resourcesCount / 4);
      stone = resourcesCount / 4;
    }
    //everzbody take 1 soul
    ResourcesManager.AddSouls(1);
    ResourcesManager.AddResources(wood, stone, lootMultiplayer);
  }

  private void DestroyLootObject() {
    //animation...
    Destroy(gameObject, timeToDestroy);
  }
}
