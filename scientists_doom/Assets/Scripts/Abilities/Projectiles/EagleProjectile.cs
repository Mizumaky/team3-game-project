using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleProjectile : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private float _radius;
  public float radius { get { return _radius; } }
  private float _duration;
  public float duration { get { return _duration; } }

  private Vector3 destination;
  private Vector3 destinationGround;
  private float distanceToDestination;
  private float destinationDistanceThreshold = 1f;

  private Vector3 direction;
  private float velocity;
  private bool isTraveling;

  private float updateInterval = 0.5f;
  private float remainingDuration;

  private GameObject areaOutlinePrefab;
  private Transform casterTransform;

  #endregion

  /// <summary>
  /// Sets the projectile's parameters and sends it towards destination
  /// </summary>
  /// <param name="destination"></param>
  /// <param name="destinationGround"></param>
  /// <param name="duration"></param>
  /// <param name="casterTransform"></param>
  public void SetAndRelease(Vector3 destination, Vector3 destinationGround, float velocity, float radius, float duration, Transform casterTransform, GameObject areaOutlinePrefab)
  {
    this.destination = destination;
    this.destinationGround = destinationGround;
    this.velocity = velocity;
    this._radius = radius;
    this._duration = duration;
    this.casterTransform = casterTransform;
    this.areaOutlinePrefab = areaOutlinePrefab;

    isTraveling = true;
    StartCoroutine(TranslateTowardsDestination());
  }

  private IEnumerator TranslateTowardsDestination()
  {
    direction = (destination - transform.position).normalized;
    transform.GetChild(0).rotation = Quaternion.LookRotation(direction);
    while (isTraveling)
    {
      transform.Translate(direction * Time.deltaTime * velocity);

      distanceToDestination = Vector3.Distance(transform.position, destination);
      // Stop when close to the destination
      if (distanceToDestination < destinationDistanceThreshold)
      {
        isTraveling = false;
        StartCoroutine(Hover());
      }

      yield return null;
    }
  }

  private IEnumerator Hover()
  {
    GameObject areaOutline = SpawnAreaOutline();

    WaitForSeconds waitInterval = new WaitForSeconds(updateInterval);
    remainingDuration = duration;

    int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
    Collider[] hits;
    Transform transformToAggroTo = transform;


    while (remainingDuration > 0)
    {
      remainingDuration -= updateInterval;
      // Set aggro to the caster on the last check
      if (remainingDuration <= updateInterval)
      {
        transformToAggroTo = casterTransform;
      }

      hits = Physics.OverlapSphere(destinationGround, radius, enemyLayerMask);
      foreach (Collider hit in hits)
      {
        hit.GetComponent<EnemyControls>().AggroTo(transformToAggroTo);
      }

      // TODO: Hover around in circles

      yield return waitInterval;
    }

    Destroy(areaOutline);
    Destroy(gameObject);
    // TODO: Make the eagle work like a boomerang
  }

  private GameObject SpawnAreaOutline()
  {
    Quaternion outlineRotation = Quaternion.identity;

    Ray rayDown = new Ray(destinationGround + Vector3.up, Vector3.down);
    RaycastHit hit;
    if (Physics.Raycast(rayDown, out hit, 5f, 1 << LayerMask.NameToLayer("Ground")))
    {
      outlineRotation = Quaternion.LookRotation(hit.normal);
    }

    GameObject areaOutline = Instantiate(areaOutlinePrefab, destinationGround, outlineRotation, transform);
    var shape = areaOutline.GetComponent<ParticleSystem>().shape;
    shape.radius = radius;
    return areaOutline;
  }
}