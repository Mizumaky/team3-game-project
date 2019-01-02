using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FireballProjectile : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private float _damage;
  public float damage { get { return _damage; } }

  private Transform casterTransform;
  private LayerMask collisionMask;

  private float travelHeight;
  private bool isTraveling;
  #endregion

  public void Set(float damage, Transform casterTransform, float travelHeight, LayerMask collisionMask)
  {
    this._damage = damage;
    this.casterTransform = casterTransform;
    this.travelHeight = travelHeight;
    this.collisionMask = collisionMask;

    isTraveling = true;
    StartCoroutine(FollowTerrain());
  }

  private IEnumerator FollowTerrain()
  {
    Rigidbody rigidbody = GetComponent<Rigidbody>();
    Ray rayDown;
    RaycastHit hit;
    float rayLength = 10f;
    int layerMask = 1 << LayerMask.NameToLayer("Ground");

    Vector3 lastPosition;

    transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
    while (isTraveling)
    {
      lastPosition = transform.position;

      rayDown = new Ray(transform.position + Vector3.up * 5f, Vector3.down);
      if (Physics.Raycast(rayDown, out hit, rayLength, layerMask))
      {
        transform.position = new Vector3(transform.position.x, travelHeight + hit.point.y, transform.position.z);
      }

      yield return null;
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (isTraveling && UnityExtensions.ContainsLayer(collisionMask, other.gameObject.layer))
    {
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
        other.GetComponent<EnemyControls>().AggroTo(casterTransform);
        other.GetComponent<EnemyStats>().TakeDamage(damage);
      }

      isTraveling = false;

      transform.GetChild(0).gameObject.SetActive(true);
      transform.GetChild(0).localScale = transform.localScale;
      HitEnemies();

      Disable();
    }
  }

  private void Disable()
  {
    Rigidbody rigidbody = GetComponent<Rigidbody>();
    if (rigidbody != null)
    {
      rigidbody.useGravity = false;
      rigidbody.velocity = Vector3.zero;
    }

    GetComponent<Collider>().enabled = false;

    Destroy(gameObject, 3);
  }

  private void HitEnemies()
  {
    int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    Collider[] hits = Physics.OverlapSphere(transform.position, transform.localScale.x, enemyLayer);

    foreach (Collider hit in hits)
    {
      hit.GetComponent<Stats>().TakeDamage(damage);
      hit.GetComponent<EnemyControls>().AggroTo(casterTransform);
    }
  }
}