using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
  public static int enemiesAlive;

  public Transform spawnOriginTransform;
  public GameObject[] enemyPrefab;

  public float spawnCircleRadius;
  [Range(1, 16)] public int numberOfWaves;
  [Range(1, 10)] public int initialGroupDelay;
  [Range(0, 0.5f)] public float individualEnemySpawnDelay;
  public AnimationCurve groupDelayDumpCurve;

  private float ringAngleOffsetLimit = 180f;
  private int totalEnemyCount = 0;
  private Coroutine activeSpawnRoutine;
  private float lastAngle = 0, nextAngle, progress, delay;
  private int levelNum;

  private void Awake() {
    if (spawnOriginTransform == null) {
      Debug.LogWarning("EnemySpawner: No spawn origin set! Setting default value!");
      spawnOriginTransform = transform;
      enemiesAlive = 0;
    }
  }

  public void StartSpawnWaveIfInactive() {
    if (activeSpawnRoutine == null) {
      activeSpawnRoutine = StartCoroutine(SpawnWave(0,0,0));
    } else {
      Debug.Log("Wave is already in progress!");
    }
  }

  public void StartSpawnWaveIfInactive(StoryLevel level) {
    if (activeSpawnRoutine == null) {
      enemiesAlive = 0;
      levelNum = level.levelNo;
      activeSpawnRoutine = StartCoroutine(SpawnWave(level.peasantCount, level.supportCount, level.siegeCount));
    } else {
      Debug.Log("Wave is already in progress!");
    }
  }

  private Vector3 GetPosition() {
    Vector3 rayOriginPosition = Vector3.zero, spawnPosition;
    float x, z;
    Ray rayDown;
    RaycastHit hit;
    float rayLength = 25f;
    int layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));

    nextAngle = (lastAngle + Random.Range(90f, 180f)) % 360f;
    lastAngle = nextAngle;

    x = spawnOriginTransform.position.x + spawnCircleRadius * Mathf.Sin(nextAngle * Mathf.Deg2Rad);
    z = spawnOriginTransform.position.z + spawnCircleRadius * Mathf.Cos(nextAngle * Mathf.Deg2Rad);
    rayOriginPosition = new Vector3(x, spawnOriginTransform.position.y + 5, z);

    rayDown = new Ray(rayOriginPosition, Vector3.down);
    spawnPosition = Vector3.zero;

    if (Physics.Raycast(rayDown, out hit, rayLength, layerMask)) {
      if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        spawnPosition = hit.point;
    }
    return spawnPosition;
  }

  private IEnumerator SpawnWave(int peasantCnt, int suppCnt, int siegeCnt) {
    //print("Will Spawn " + peasantCnt + " + " + suppCnt + " + " + siegeCnt + " = " + (peasantCnt+suppCnt+siegeCnt));
    int enemiesLeft = peasantCnt + suppCnt + siegeCnt;
    enemiesAlive = enemiesLeft;

    int[] enemyTypesCnt = new int[3];
    enemyTypesCnt[0] = peasantCnt;
    enemyTypesCnt[1] = suppCnt;
    enemyTypesCnt[2] = siegeCnt;

    int[] groupSize = new int[3];
    for (int i = 0; i < groupSize.Length; i++) {
      groupSize[i] = enemyTypesCnt[i] / numberOfWaves;
      if (groupSize[i] == 0 && enemyTypesCnt[i] != 0)
        groupSize[i] = 1;
    }
    Vector3 spawnPosition = Vector3.zero;

    while (enemiesLeft > 0) {
      for (int i = 0; i < groupSize.Length; i++) {
        if (enemyTypesCnt[i] <= 0) {
          continue;
        }
        spawnPosition = GetPosition();

        if (spawnPosition == Vector3.zero) {
          Debug.LogWarning("EnemySpawner: Can't find group spawn via raycast! Picking new location!");
          i--;
          continue;
        }
        
        StartCoroutine(SpawnEnemyGroup((groupSize[i] > enemyTypesCnt[i] ? enemyTypesCnt[i] : groupSize[i]), spawnPosition, i));
        enemyTypesCnt[i] -= groupSize[i];
        enemiesLeft -= groupSize[i];
        progress = 1f - (float)enemiesLeft / (float)enemiesAlive;

        Debug.Log("Level Progress: " + progress);

        delay = initialGroupDelay * groupDelayDumpCurve.Evaluate(progress);
        yield return new WaitForSeconds(initialGroupDelay * groupDelayDumpCurve.Evaluate(progress));
      }      
    }
    levelNum++;
    activeSpawnRoutine = null;
    yield return null;
  }

  private IEnumerator SpawnEnemyGroup(int groupSize, Vector3 groupSpawnPosition, int enemy_type) {
    Quaternion rotation = Quaternion.LookRotation((transform.position - groupSpawnPosition).normalized);

    WaitForSeconds delay = new WaitForSeconds(individualEnemySpawnDelay);
    for (int i = 0; i < groupSize; i++) {
      GameObject enemy = Instantiate(enemyPrefab[enemy_type], groupSpawnPosition, rotation, transform);
      enemy.transform.localScale = enemy.transform.localScale * Random.Range(1.4f, 1.7f);
      yield return delay;
    }
    yield return null;
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, spawnCircleRadius);
  }
}
