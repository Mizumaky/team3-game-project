using UnityEngine;

public class Explosive : MonoBehaviour {
  public GameObject explosion;
  public Vector3[] explosionSpawnPoints;

  public float explosionScale;

  public bool enableCollision = true;

  private int chainExplosionLength = 0; 

  public void Explode (int chainLength) {
    Debug.Log ("Eh");
    chainExplosionLength = chainLength;
    foreach (Vector3 spawnPoint in explosionSpawnPoints) {
      GameObject newExplosion = Instantiate (explosion, transform.position, Quaternion.identity);
      newExplosion.transform.localScale *= explosionScale;
      CheckHitsAndDealDamage (spawnPoint);
    }
    Destroy (gameObject);
  }
    public void Explode()
    {
        chainExplosionLength = 0;
        Debug.Log("Eh");
        foreach (Vector3 spawnPoint in explosionSpawnPoints)
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.transform.localScale *= explosionScale;
            CheckHitsAndDealDamage(spawnPoint);
        }
        Destroy(gameObject);
    }


    private void CheckHitsAndDealDamage (Vector3 spawnPoint) {
    Debug.Log ("H");
    Collider[] hits = Physics.OverlapSphere (transform.position, explosionScale * 0.5f);

    int enemyLayer = LayerMask.NameToLayer ("Enemy");
    int explosiveLayer = LayerMask.NameToLayer("Explosive");
    foreach (Collider hit in hits) {
            if (hit.gameObject.layer == enemyLayer)
            {
                hit.GetComponent<EnemyStats>().TakeDamage(500f);
            }
            else if (hit.gameObject.layer == explosiveLayer && hit.gameObject != gameObject) {
                if (chainExplosionLength < 5)
                {
                    hit.GetComponent<Explosive>().Explode(chainExplosionLength+1);
                }
            }
    }

    
}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f * explosionScale);
    }
}