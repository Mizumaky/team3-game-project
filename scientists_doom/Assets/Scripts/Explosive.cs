using UnityEngine;

public class Explosive : MonoBehaviour {
  public GameObject explosion;
  public Vector3[] explosionSpawnPoints;

  public float explosionScale;

  public bool enableCollision = true;

  public void Explode () {
    Debug.Log ("Eh");
    foreach (Vector3 spawnPoint in explosionSpawnPoints) {
      GameObject newExplosion = Instantiate (explosion, transform.position, Quaternion.identity);
      newExplosion.transform.localScale *= explosionScale;
      CheckHitsAndDealDamage (spawnPoint);
    }
    Destroy (gameObject, explosion.GetComponent<ParticleSystem> ().main.startLifetime.Evaluate (0));
  }

  private void CheckHitsAndDealDamage (Vector3 spawnPoint) {
    Debug.Log ("H");
    Collider[] hits = Physics.OverlapSphere (transform.position, explosionScale * 0.5f);

    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    foreach (Collider hit in hits) {
      if (hit.gameObject.layer == enemyLayer) {
        hit.GetComponent<EnemyStats> ().TakeDamage (500f);
      }
    }
  }
}