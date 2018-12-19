using System.Collections.Generic;
using UnityEngine;

public class DoubleFocusCamera : MonoBehaviour
{

  [Header("Camera Settings")]
  public Transform focus;
  public Transform secondaryFocus;

  [SerializeField] private float defaultRadius = 20f;
  [SerializeField] private float fovSmoothTime = .5f;
  [SerializeField] private float posSmoothTime = .5f;
  [SerializeField] private float rotSlerpParam = .3f;
  [SerializeField] private int minFOV = 35;
  [SerializeField] private int maxFOV = 60;
  [SerializeField] private AnimationCurve heightCurve;

  [Space]
  [Tooltip("When camera crosses playerBounds distance from the castle, it starts to move with the player")]
  [SerializeField] private float playerBounds = 5f;

  //leave here for debugging
  private float diameter;

  private float scrollWheel = 0.5f;

  //for smoothDaping
  private Vector3 velocity;
  private float rotVelocity;
  private float FOVvelocity;

  private float lerpTime = 0;


  /*public void SetFocusTransform(Transform focus) {
      this.focus = focus;
  }*/

  public void UpdateDFCamera()
  {
    //get scroll wheel value
    if (Input.GetAxis("scrollwheel") > 0 && scrollWheel < 1f)
    {
      scrollWheel += 0.2f;

    }
    else if (Input.GetAxis("scrollwheel") < 0 && scrollWheel >= 0.1f)
    {
      scrollWheel -= 0.2f;

    }

    float currentFOV = gameObject.GetComponent<Camera>().fieldOfView;
    float targetFOV = minFOV + Mathf.Lerp(0, maxFOV - minFOV, scrollWheel);

    gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(currentFOV, targetFOV, ref FOVvelocity, fovSmoothTime);


    float playerDistance = Vector3.Distance(secondaryFocus.position, focus.position);

    //camera position modifiers
    float distanceScale;
    float cameraHeight = heightCurve.Evaluate(playerDistance);



    diameter = defaultRadius;

    if (playerDistance > playerBounds)
    {
      float k = Mathf.Lerp(0, 12, 1 - playerBounds / playerDistance);
      diameter = defaultRadius + k;
    }

    distanceScale = diameter / playerDistance;

    Vector3 newPosition = new Vector3(distanceScale * focus.position.x,
                                      cameraHeight,
                                      distanceScale * focus.position.z);
    Vector3 cameraPos = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, posSmoothTime);
    transform.position = cameraPos;

    Vector3 direction = (focus.position - transform.position).normalized;
    Quaternion newRotation = Quaternion.LookRotation(direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotSlerpParam);
  }
}
