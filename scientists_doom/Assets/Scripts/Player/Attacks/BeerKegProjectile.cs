using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerKegProjectile : MonoBehaviour
{
  [Header("Travel")]
  public Vector3 destination;
  public float timeToLive;
  public float speed;
  public AnimationCurve travelHeightPercentage;

  private Vector3 direction;
  private float overcomeDistance;
  private float totalDistance;

  [Header("Hit")]
  public int damageOnSplash;
  public float spillDecayTime;
  public GameObject spillPrefab;

  [Header("Carry")]
  public Transform casterTransform;

  private void Start()
  {
    totalDistance = Vector3.Distance(transform.position, destination);
    overcomeDistance = 0;

    direction = (destination - transform.position).normalized;

    Destroy(gameObject, timeToLive);
  }

  private void Update()
  {
    Vector3 targetPositionBase = transform.position + direction * Time.deltaTime * speed;
    float curveX = totalDistance / Vector3.Distance(targetPositionBase, transform.position);
    Debug.Log(curveX);
    if (curveX <= 1)
    {
      float y = targetPositionBase.y + travelHeightPercentage.Evaluate(curveX);
      Vector3 targetPosition = new Vector3(targetPositionBase.x, y, targetPositionBase.z);

      transform.Translate(targetPosition);
    }
  }
}