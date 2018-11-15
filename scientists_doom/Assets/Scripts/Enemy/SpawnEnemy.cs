using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

  public GameObject enemyPrefab;
  public int enemyCount;
  public float radius;
  public float timeBetweenWaves = 5f;
  public AnimationCurve curve;

  private float ringAngleOffsetLimit = 180f;
  private float lastAngle = 0f;

  private Coroutine wave;

  void Start () {
    wave = StartCoroutine (SpawnWave ());
  }

  public void SpawnWaveIfNotInProgress () {
    if (wave == null) {
      wave = StartCoroutine (SpawnWave ());
    } else {
      Debug.Log ("Wave is already in progress!");
    }
  }

  IEnumerator SpawnWave () {
    float numOfEnemies;
    float value = 0;
    int enemiesSpawned = 0;
    while (value < 1) {
      value += 0.1f;
      numOfEnemies = (curve.Evaluate (value) * enemyCount) - enemiesSpawned;
      enemiesSpawned += (int) numOfEnemies;
      SpawnEnemyGroup ((int) numOfEnemies);
      yield return new WaitForSeconds (timeBetweenWaves);
    }
    yield return null;
    wave = null;
    }

  private void SpawnEnemyGroup (int count) {
    Vector3 center = transform.position;
    float nextAngle = (lastAngle + Random.Range (90f, 180f)) % 360f;
    lastAngle = nextAngle;

    // create a ray to find enemy y pos
    float rayPosX = transform.position.x + radius * Mathf.Sin (nextAngle * Mathf.Deg2Rad);
    float rayPosZ = transform.position.z + radius * Mathf.Cos (nextAngle * Mathf.Deg2Rad);
    float rayLength = 50f;
    Vector3 rayOriginPosition = new Vector3 (rayPosX, transform.position.y + 10, rayPosZ);
    Ray rayDown = new Ray (rayOriginPosition, Vector3.down);

    // spawn enemy on the intersection of the ray and terrain
    RaycastHit hit;
    if (Physics.Raycast (rayDown, out hit, rayLength)) {
      if (hit.collider.gameObject.layer == 9) {
        Quaternion rot = Quaternion.LookRotation ((center - hit.point).normalized);
        for (int i = 0; i < count; i++) {
          GameObject enemy = Instantiate (enemyPrefab, hit.point, rot);
          enemy.transform.parent = gameObject.transform;
        }
      }
    }

  }
}