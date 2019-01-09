using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ArrowProjectile : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private float _damage;
  public float damage { get { return _damage; } }
  private int _damageEmpowered;
  public int damageEmpowered { get { return _damageEmpowered; } }

  private Transform casterTransform;
  private bool isEmpowered;
  private LayerMask collisionMask;

  private bool isTraveling;
  private float travelHeight;
  public float timeToLive = 2;
  #endregion

  public void Set(float damage, bool isEmpowered, float timeToLive, Transform casterTransform, float travelHeight, LayerMask collisionMask)
  {
    this._damage = damage;
    this.isEmpowered = isEmpowered;
    this.timeToLive = timeToLive;
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

  private void OnTriggerEnter(Collider other)
  {
    if (isTraveling && UnityExtensions.ContainsLayer(collisionMask, other.gameObject.layer))
    {
      // FIXME: NameToLayer can be optimized
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
      {
        other.GetComponent<EnemyControls>().AggroTo(casterTransform);

        other.GetComponent<EnemyStats>().TakeDamage(damage);

        if (isEmpowered)
        {
          return;
        }
      }

      isTraveling = false;

      Rigidbody rigidbody = GetComponent<Rigidbody>();
      if (rigidbody != null)
      {
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
      }

      GetComponent<Collider>().enabled = false;

      transform.parent = other.transform;
      Destroy(gameObject, 3);

    }
  }
}