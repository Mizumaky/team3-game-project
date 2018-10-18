using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public GameObject enemyPrefab;
    public int enemyCount;
    public float radius;
    public float maxTimeBetweenWaves; //
    public AnimationCurve curve;
    [Range(0, 180f)]
    public float ringAngleOffsetLimit;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        float numOfEnemies;
        float value = 0;
        int enemiesSpawned = 0;
        while (value < 1)
        {
            value += 0.1f;
            numOfEnemies = (curve.Evaluate(value) * enemyCount) - enemiesSpawned;
            enemiesSpawned += (int)numOfEnemies;
            SpawnEnemyInRing((int)numOfEnemies);
            print("Enemies spawned now " + numOfEnemies + " " + enemiesSpawned + "/" + enemyCount);
            yield return new WaitForSeconds(maxTimeBetweenWaves);
        }
    }

    private void SpawnEnemyInRing(int count)
    {
        Vector3 center = transform.position;
        float accurateAngle = 360f / enemyCount;
        float ringAngleOffset;
        float offsetAngle;
        for (int i = 0; i < count; i++)
        {
            // enemies will spawn on random place in ring
            ringAngleOffset = Random.Range(-ringAngleOffsetLimit, ringAngleOffsetLimit);
            offsetAngle = (accurateAngle * (i + 1) + ringAngleOffset) % 360;

            // create a ray to find enemy y pos
            float rayPosX = transform.position.x + radius * Mathf.Sin(offsetAngle * Mathf.Deg2Rad);
            float rayPosZ = transform.position.z + radius * Mathf.Cos(offsetAngle * Mathf.Deg2Rad);
            float rayLength = 50f;
            Vector3 rayOriginPosition = new Vector3(rayPosX, transform.position.y + 10, rayPosZ);
            Ray rayDown = new Ray(rayOriginPosition, Vector3.down);

            // spawn enemy on the intersection of the ray and terrain
            RaycastHit hit;
            if (Physics.Raycast(rayDown, out hit, rayLength))
            {
                if (hit.collider.gameObject.layer == 9)
                { 
                    Quaternion rot = Quaternion.LookRotation((center - hit.point).normalized);
                    Instantiate(enemyPrefab, hit.point, rot);
                }
            }
        }
    }
}
