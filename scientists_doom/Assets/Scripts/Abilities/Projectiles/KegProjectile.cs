using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegProjectile : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }
  private float _spillDuration;
  public float spillDuration { get { return _spillDuration; } }

  private GameObject spillPrefab;
  private Transform casterTransform;
  private bool isTraveling;
  private LayerMask collisionMask;

  #endregion

  /// <summary>
  /// Sets the projectile's parameters and sends it towards destination
  /// </summary>
  /// <param name="destination"></param>
  /// <param name="destinationGround"></param>
  /// <param name="duration"></param>
  /// <param name="casterTransform"></param>
  public void SetAndRelease(int damage, float spillDuration, GameObject spillPrefab, Transform casterTransform, LayerMask collisionMask)
  {
    this._damage = damage;
    this._spillDuration = spillDuration;
    this.spillPrefab = spillPrefab;
    this.casterTransform = casterTransform;
    this.collisionMask = collisionMask;

    isTraveling = true;
    StartCoroutine(Rotation());
  }

  private IEnumerator Rotation()
  {
    Vector3 rotationVector = new Vector3(2, 1, 0);

    while (isTraveling)
    {
      transform.Rotate(rotationVector);
      yield return null;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (UnityExtensions.ContainsLayer(collisionMask, other.gameObject.layer))
    {
      isTraveling = false;

      GetComponent<ParticleSystem>().Play();

      SpawnSpill();
      Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
  }

  private void SpawnSpill()
  {
    Ray rayDown = new Ray(transform.position + Vector3.up, Vector3.down);
    RaycastHit hit;

    HitEnemies();

    if (Physics.Raycast(rayDown, out hit, 3f, 1 << LayerMask.NameToLayer("Ground")))
    {
      GameObject newSpill = Instantiate(spillPrefab, hit.point, Quaternion.LookRotation(hit.normal), null);
      Destroy(newSpill, spillDuration);
    }
  }

  private void HitEnemies()
  {
    float radius = 3;
    int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

    foreach (Collider hit in hits)
    {
      hit.GetComponent<Stats>().TakeDamage(damage);
      hit.GetComponent<EnemyControls>().AggroTo(casterTransform);
    }
  }

}