using UnityEngine;

public class Explosive : MonoBehaviour {
	public GameObject explosion;
	public Vector3[] explosionSpawnPoints;
	public float explosionScale;
	public bool enableCollision = true;
	public bool hasExploded;

	public void Explode () {
		if (!hasExploded) {
			hasExploded = true;
			foreach (Vector3 spawnPoint in explosionSpawnPoints) {
				GameObject newExplosion = Instantiate (explosion, transform.position, Quaternion.identity);
				newExplosion.transform.localScale *= explosionScale;
				CheckHitsAndDealDamage (spawnPoint);
			}
			Destroy (gameObject);
		}
	}
	private void CheckHitsAndDealDamage (Vector3 spawnPoint) {
		Collider[] hits = Physics.OverlapSphere (transform.position, explosionScale * 0.5f);

		int enemyLayer = LayerMask.NameToLayer ("Enemy");
		int explosiveLayer = LayerMask.NameToLayer ("Explosive");
		foreach (Collider hit in hits) {
			if (hit.gameObject.layer == enemyLayer) {
				hit.GetComponent<EnemyStats> ().TakeDamage (500f);
			} else if (hit.gameObject.layer == explosiveLayer && hit.gameObject != gameObject) {
				hit.GetComponent<Explosive> ().Explode ();
			}
		}

	}
	private void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, 0.5f * explosionScale);
	}
}