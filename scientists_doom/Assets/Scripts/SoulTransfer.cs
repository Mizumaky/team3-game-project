using UnityEngine;

public class SoulTransfer : MonoBehaviour {
  public AnimationCurve travelHeightCurve;
  public float travelTime, travelHeightMultiplier, travelProgress;

  public Transform targetTransform;
  private Vector3 originPosition, targetPosition;
  private float totalDistance;
  private Vector3 direction;
  private float step;

  private void Start () {
    originPosition = transform.position;
    targetPosition = targetTransform.position;

    totalDistance = Vector3.Distance (originPosition, targetPosition);
    direction = (targetPosition - originPosition).normalized;
    step = totalDistance / (travelTime / Time.deltaTime);
  }

  private void Update () {
    transform.position += step * direction;
    travelProgress += step / totalDistance;
  }
}