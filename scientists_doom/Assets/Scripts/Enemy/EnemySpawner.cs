using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  public Transform spawnOriginTransform;
  public GameObject enemyPrefab;

  [Space]
  public bool spawnOnStart = false;
  public float spawnCircleRadius;
  [Range(1, 1000)] public int totalEnemyCount;
  [Range(1, 16)] public int numberOfWaves;
  [Range(1, 10)] public int initialGroupDelay;
  [Range(0, 0.5f)] public float individualEnemySpawnDelay;
  public AnimationCurve groupDelayDumpCurve;

  private float ringAngleOffsetLimit = 180f;
  private Coroutine activeSpawnRoutine;

  private void Awake()
  {
    if (spawnOriginTransform == null)
    {
      Debug.LogWarning("EnemySpawner: No spawn origin set! Setting default value!");
      spawnOriginTransform = transform;
    }
  }
  private void Start()
  {
    if (spawnOnStart)
    {
      activeSpawnRoutine = StartCoroutine(SpawnWave(totalEnemyCount));
    }
  }

  public void StartSpawnWaveIfInactive()
  {
    if (activeSpawnRoutine == null)
    {
      activeSpawnRoutine = StartCoroutine(SpawnWave(totalEnemyCount));
    }
    else
    {
      Debug.Log("Wave is already in progress!");
    }
  }

  public void StartSpawnWaveIfInactive(int enemyCount)
  {
    if (activeSpawnRoutine == null)
    {
      activeSpawnRoutine = StartCoroutine(SpawnWave(enemyCount));
    }
    else
    {
      Debug.Log("Wave is already in progress!");
    }
  }

  private IEnumerator SpawnWave(int enemyCount)
  {
    int enemiesLeft = enemyCount;
    int groupSize = totalEnemyCount / numberOfWaves;
    if (groupSize == 0)
    {
      groupSize = 1;
    }

    Vector3 rayOriginPosition = Vector3.zero, spawnPosition;
    float x, z, lastAngle = 0, nextAngle, progress, delay;
    Ray rayDown;
    RaycastHit hit;
    float rayLength = 25f;
    int groundLayer = 1 << LayerMask.NameToLayer("Ground");

    while (enemiesLeft > 0)
    {
      nextAngle = (lastAngle + Random.Range(90f, 180f)) % 360f;
      lastAngle = nextAngle;

      x = spawnOriginTransform.position.x + spawnCircleRadius * Mathf.Sin(nextAngle * Mathf.Deg2Rad);
      z = spawnOriginTransform.position.z + spawnCircleRadius * Mathf.Cos(nextAngle * Mathf.Deg2Rad);
      rayOriginPosition = new Vector3(x, spawnOriginTransform.position.y, z);

      rayDown = new Ray(rayOriginPosition, Vector3.down);
      spawnPosition = Vector3.zero;

      if (Physics.Raycast(rayDown, out hit, rayLength, groundLayer))
      {
        spawnPosition = hit.point;
      }

      if (spawnPosition == Vector3.zero)
      {
        Debug.LogWarning("EnemySpawner: Can't find group spawn via raycast! Picking new location!");
        continue;
      }
      StartCoroutine(SpawnEnemyGroup(groupSize, hit.point));

      enemiesLeft -= groupSize;
      progress = enemiesLeft / totalEnemyCount;

      delay = initialGroupDelay * groupDelayDumpCurve.Evaluate(progress);
      yield return new WaitForSeconds(initialGroupDelay * groupDelayDumpCurve.Evaluate(progress));
    }

    activeSpawnRoutine = null;
    yield return null;
  }

  private IEnumerator SpawnEnemyGroup(int groupSize, Vector3 groupSpawnPosition)
  {
    Quaternion rotation = Quaternion.LookRotation((transform.position - groupSpawnPosition).normalized);

    WaitForSeconds delay = new WaitForSeconds(individualEnemySpawnDelay);
    for (int i = 0; i < groupSize; i++)
    {
      GameObject enemy = Instantiate(enemyPrefab, groupSpawnPosition, rotation, transform);
      enemy.transform.localScale = enemy.transform.localScale * Random.Range(0.8f, 2f);
      yield return delay;
    }

    yield return null;
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, spawnCircleRadius);
  }
}