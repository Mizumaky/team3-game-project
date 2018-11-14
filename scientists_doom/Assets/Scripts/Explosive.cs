using UnityEngine;

/// <summary>
/// Adds explosion functionality to GameObject
/// </summary>
public class Explosive : MonoBehaviour {

  [Header ("Explosion")]
  [SerializeField] private LayerMask hitMask;
  [SerializeField] private GameObject explosionPrefab;
  [Space]
  [SerializeField] private float explosionRadius;
  [Space]
  [SerializeField] private float baseExplosionDamage = 100f;

  [Header ("Object")]
  [SerializeField] private bool enableCollision = true;
  private bool hasExploded = false;

  /// <summary>
  /// Spawns explosion particle effects, hits surroundings and destroys the object
  /// </summary>
  public void Explode (Transform casterTransform) {
    if (hasExploded) return;

    hasExploded = true;
    Vector3 position = transform.position;

    // Spawn explosion
    GameObject newExplosion = Instantiate (explosionPrefab, position, Quaternion.identity);
    newExplosion.transform.localScale *= explosionRadius / 0.5f;
    Hit (position, explosionRadius, casterTransform);

    // TODO: Object destrucion animations
    Destroy (gameObject, explosionPrefab.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void Hit (Vector3 spawnPoint, float explosionRadius, Transform casterTransform) {
    Collider[] hits = Physics.OverlapSphere (transform.position, explosionRadius, hitMask);

    // Layers
    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    int explosiveLayer = LayerMask.NameToLayer ("Explosive");

    foreach (Collider hit in hits) {
      if (hit.gameObject.layer == enemyLayer) {
        hit.GetComponent<EnemyStats> ().TakeDamage (baseExplosionDamage);
      }

      if (hit.gameObject.layer == explosiveLayer) {
        hit.GetComponent<Explosive> ().Explode (casterTransform);
      }
    }
  }
}