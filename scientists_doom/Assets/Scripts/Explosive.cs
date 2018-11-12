using UnityEngine;

/// <summary>
/// Adds explosion functionality to GameObject
/// </summary>
public class Explosive : MonoBehaviour {

  [Header ("Explosion")]
  [SerializeField] private GameObject explosionPrefab;
  [Space]
  [SerializeField] private bool spawnAtObjectPosition;
  [Space]
  [SerializeField] private Vector3[] explosionSpawnPoints;
  [SerializeField] private float[] explosionsScale;
  [SerializeField] private float nextExplosionDelay;
  [Space]
  [SerializeField] private float baseExplosionDamage = 100f;

  [Header ("Object")]
  [SerializeField] private bool enableCollision = true;

  private void Awake () {
    explosionsScale = new float[explosionSpawnPoints.Length];
  }

  /// <summary>
  /// Spawns explosion particle effects, hits surroundings and destroys the object
  /// </summary>
  public void Explode () {
    // Check if solo explosion, set its origin to object's position
    if (spawnAtObjectPosition) {
      explosionSpawnPoints = new Vector3[1];
      explosionSpawnPoints[0] = transform.position;
    }
    // Spawn explosions, hit in each exp. area
    for (int i = 0; i < explosionSpawnPoints.Length; i++) {
      GameObject newExplosion = Instantiate (explosionPrefab, explosionSpawnPoints[i], Quaternion.identity);
      newExplosion.transform.localScale *= explosionsScale[i];
      CheckHitsAndDealDamage (explosionSpawnPoints[i], explosionsScale[i]);
    }
    // Destroy G.O.
    // TODO: Object destrucion animations
    Destroy (gameObject, explosionPrefab.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void CheckHitsAndDealDamage (Vector3 spawnPoint, float explosionScale) {
    Collider[] hits = Physics.OverlapSphere (transform.position, explosionScale * 0.5f);

    // Layers
    int enemyLayer = LayerMask.NameToLayer ("Enemy");

    foreach (Collider hit in hits) {
      if (hit.gameObject.layer == enemyLayer) {
        hit.GetComponent<EnemyStats> ().TakeDamage (500f);
      }
    }
  }
}