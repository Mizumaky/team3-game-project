using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {
  public float timeToDestroy = 0.0f;
  public int resourcesCount;
  private float lootMultiplayer;

	// Use this for initialization
	void Start () {
    lootMultiplayer = Random.Range(0.85f, 1.2f);
    resourcesCount = (int)(resourcesCount*lootMultiplayer);
  }

  public void DestroyLootObject() {
    //animation...
    Destroy(gameObject, timeToDestroy);
  }
}
