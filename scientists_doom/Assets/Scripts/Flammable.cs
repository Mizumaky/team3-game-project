using System.Collections;
using UnityEngine;

/// <summary>
/// Adds explosion functionality to GameObject
/// </summary>
public class Flammable : MonoBehaviour {

  [Header ("Fire")]
  [SerializeField] private GameObject firePrefab;
  [Space]
  [SerializeField] private bool spawnAtObjectPosition;
  [Space]
  [SerializeField] private Vector3[] fireSpawnPoints;
  [SerializeField] private float[] fireScale;

  [Header ("Object")]
  [SerializeField] private GameObject currentObjectModel;
  [SerializeField] private GameObject[] objectBurnStagePrefabs;

  private void Awake () {
    fireScale = new float[fireSpawnPoints.Length];
  }

  /// <summary>
  /// Spawns fire at different positions
  /// </summary>
  public void CatchFire () {
    // Spawn fires
    for (int i = 0; i < fireSpawnPoints.Length; i++) {
      GameObject newFire = Instantiate (firePrefab, fireSpawnPoints[i], Quaternion.identity, transform);
      newFire.transform.localScale *= fireScale[i];
    }
  }

  private IEnumerator StageChangeTimer (float time) {
    int index = 0;
    WaitForSeconds period = new WaitForSeconds (time);
    while (index < objectBurnStagePrefabs.Length) {
      yield return period;
      SwapModels (index++);
    }
  }

  private void SwapModels (int index) {
    Transform previousObjectTransform = currentObjectModel.transform;

    currentObjectModel = Instantiate (objectBurnStagePrefabs[index], previousObjectTransform.position, previousObjectTransform.rotation, transform);

    Destroy (previousObjectTransform.gameObject);
  }
}