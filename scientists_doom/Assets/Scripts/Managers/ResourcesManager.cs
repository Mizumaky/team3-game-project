using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {
  [HideInInspector]
  public static int wood;
  [HideInInspector]
  public static int stone;
  [HideInInspector]
  public static int souls;
  public HUDScript HUD;

  void Start () {
    wood = 0;
    stone = 0;
    souls = 0;
  }

  public static void AddResources (int _wood, int _stone, float randomMultiplier) {
    wood += (int) (_wood * randomMultiplier);
    stone += (int) (_stone * randomMultiplier);
    Debug.Log ("Adding wood " + _wood + " stone " + _stone);
  }

  public static void TakeResources (int _wood, int _stone) {
    wood -= _wood;
    stone -= _stone;
  }

  public static void AddSouls (int _souls) {
    souls += _souls;
  }

  public static void TakeSouls (int _souls) {
    souls -= _souls;
  }

}