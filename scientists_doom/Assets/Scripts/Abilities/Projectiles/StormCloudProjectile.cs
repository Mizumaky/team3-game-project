using System.Collections;
using UnityEngine;

public class StormCloudProjectile : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private float _damagePerTick;
  public float damagePerTick { get { return _damagePerTick; } }
  private float _duration;
  public float duration { get { return _duration; } }

  private Vector3 destination;
  private Vector3 destinationGround;
  private float distanceToDestination;
  private float destinationDistanceThreshold = 1f;

  private Vector3 direction;
  private float velocity;
  private bool isTraveling;

  private float radius;
  private float hitInterval = 0.5f;
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
  public void SetAndRelease(Vector3 destination, Vector3 destinationGround, float damagePerTick, float radius, float velocity, float duration, Transform casterTransform, GameObject areaOutlinePrefab)
  {
    this.destination = destination;
    this.destinationGround = destinationGround;
    this.velocity = velocity;
    this._damagePerTick = damagePerTick;
    this.radius = radius;
    this._duration = duration;
    this.casterTransform = casterTransform;
    this.areaOutlinePrefab = areaOutlinePrefab;

    isTraveling = true;
    StartCoroutine(TranslateTowardsDestination());
  }

  private IEnumerator TranslateTowardsDestination()
  {
    direction = (destination - transform.position).normalized;
    while (isTraveling)
    {
      transform.position += direction * Time.deltaTime * velocity;

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
    // Init wait interval
    WaitForSeconds waitForHitInterval = new WaitForSeconds(hitInterval);
    remainingDuration = duration;

    // Spawn area outline
    GameObject areaOutline = null;
    Ray rayDown = new Ray(destinationGround + Vector3.up, Vector3.down);
    RaycastHit rayHit;
    var groundLayer = LayerMask.NameToLayer("Ground");
    if (Physics.Raycast(rayDown, out rayHit, 5f, 1 << groundLayer))
    {
      areaOutline = SpawnAreaOutline(rayHit.normal);
    }
    else
    {
      Debug.LogWarning("Ray missed!");
    }

    // Hit
    var enemyLayer = LayerMask.NameToLayer("Enemy");
    var explosiveLayer = LayerMask.NameToLayer("Explosive");
    Collider[] hits;
    while (remainingDuration > 0)
    {
      remainingDuration -= hitInterval;

      hits = Physics.OverlapSphere(destinationGround, radius, (1 << enemyLayer) | (1 << groundLayer));
      foreach (Collider hit in hits)
      {
        if (hit.gameObject.layer == enemyLayer)
        {
          hit.GetComponent<EnemyStats>().TakeDamage(damagePerTick);
          hit.GetComponent<EnemyControls>().AggroTo(casterTransform);
        }
        else if (hit.gameObject.layer == explosiveLayer)
        {
          hit.GetComponent<Explosive>().Explode(casterTransform);
        }
      }

      yield return waitForHitInterval;
    }

    Destroy(areaOutline);
    Destroy(gameObject);
  }

  private GameObject SpawnAreaOutline(Vector3 rotationVector)
  {
    var shape = areaOutlinePrefab.GetComponent<ParticleSystem>().shape;
    shape.radius = radius;

    var rotation = Quaternion.LookRotation(rotationVector);
    return Instantiate(areaOutlinePrefab, destinationGround, rotation, null) as GameObject;
  }
}