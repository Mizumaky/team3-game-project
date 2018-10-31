using UnityEngine;

public class Flammable : MonoBehaviour {

  public GameObject fireParticleSystem;
  public Vector3[] fireSpawnPoints;
  private int resistCharges;

  public void CheckChargesAndSetAflame () {
    if (resistCharges > 0) {
      resistCharges--;
    } else {
      foreach (Vector3 spawnPoint in fireSpawnPoints) {
        Instantiate (fireParticleSystem, spawnPoint, Quaternion.identity);
      }
    }
  }

}