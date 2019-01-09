using UnityEngine;

public class SoulTransfer : MonoBehaviour {
  public AnimationCurve travelHeightCurve;
  public float travelTime, travelHeightMultiplier, travelProgress;

  public Transform castle;

  private GameObject soulCollector;
  private Vector3 originPosition, targetPosition;
  private float totalDistance;
  private Vector3 direction;
  private float step;

  private void Start () {
    originPosition = transform.position;
    soulCollector = castle.FindDeepChild("SoulCollector").gameObject;
    targetPosition = castle.FindDeepChild("SoulCollector").transform.position;

    totalDistance = Vector3.Distance (originPosition, targetPosition);
    direction = (targetPosition - originPosition).normalized;
    step = totalDistance / (travelTime / Time.deltaTime);
  }

  private void Update () {
    if(travelProgress <= 1){
      transform.position += step * direction;
      travelProgress += step / totalDistance;
    }else{
      Debug.Log("Soul Gone");
      soulCollector.GetComponent<SoulCollectorController>().AcquireSoul();
      Destroy(gameObject);
    }
  }
}