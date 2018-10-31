using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

  public GameObject enemyPrefab;
  public int enemyCount;
  public float radius;
  public float maxTimeBetweenWaves; //
  public AnimationCurve curve;
  private float ringAngleOffsetLimit = 180f;

  void Start () {
    StartCoroutine (SpawnWave ());

  }

  IEnumerator SpawnWave () {
    float numOfEnemies;
    float value = 0;
    int enemiesSpawned = 0;
    while (value < 1) {
      value += 0.01f;
      numOfEnemies = (curve.Evaluate (value) * enemyCount) - enemiesSpawned;
      enemiesSpawned += (int) numOfEnemies;
      SpawnEnemyInRing ((int) numOfEnemies);
      yield return new WaitForSeconds (0.1f);
    }
    yield return null;
  }

  private void SpawnEnemyInRing (int count) {
    Vector3 center = transform.position;
    float accurateAngle = 360f / enemyCount;
    float ringAngleOffset;
    float offsetAngle;
    for (int i = 0; i < count; i++) {
      // enemies will spawn on random place in ring
      ringAngleOffset = Random.Range (-ringAngleOffsetLimit, ringAngleOffsetLimit);
      offsetAngle = (accurateAngle * (i + 1) + ringAngleOffset) % 360;

      // create a ray to find enemy y pos
      float rayPosX = transform.position.x + radius * Mathf.Sin (offsetAngle * Mathf.Deg2Rad);
      float rayPosZ = transform.position.z + radius * Mathf.Cos (offsetAngle * Mathf.Deg2Rad);
      float rayLength = 50f;
      Vector3 rayOriginPosition = new Vector3 (rayPosX, transform.position.y + 10, rayPosZ);
      Ray rayDown = new Ray (rayOriginPosition, Vector3.down);

      // spawn enemy on the intersection of the ray and terrain
      RaycastHit hit;
      if (Physics.Raycast (rayDown, out hit, rayLength)) {
        if (hit.collider.gameObject.layer == 9) {
          Quaternion rot = Quaternion.LookRotation ((center - hit.point).normalized);
          GameObject enemy = Instantiate (enemyPrefab, hit.point, rot);
          enemy.transform.parent = gameObject.transform;
        }
      }
    }
  }
}