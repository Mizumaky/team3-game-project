using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ArrowProjectile : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }
  private int _damageEmpowered;
  public int damageEmpowered { get { return _damageEmpowered; } }

  private Transform casterTransform;
  private bool isEmpowered;
  private LayerMask collisionMask;

  private float travelHeight;
  private bool isTraveling;
  #endregion

  /// <summary>
  /// Sets the projectile's parameters and sends it towards destination
  /// </summary>
  /// <param name="destination"></param>
  /// <param name="destinationGround"></param>
  /// <param name="duration"></param>
  /// <param name="casterTransform"></param>
  public void Set(int damage, int damageEmpowered, Transform casterTransform, float travelHeight, bool isEmpowered, LayerMask collisionMask)
  {
    this._damage = damage;
    this._damageEmpowered = damageEmpowered;
    this.casterTransform = casterTransform;
    this.travelHeight = travelHeight;
    this.isEmpowered = isEmpowered;
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

        float shotDmg = (isEmpowered ? _damageEmpowered : _damage);
        other.GetComponent<EnemyStats>().TakeDamage(shotDmg);

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