using UnityEngine;

public class RotatingCamera : MonoBehaviour {

    public enum CameraType { StreetView, Rotating }

    [Header("Camera Switch")]
    public CameraType currentCameraType = CameraType.Rotating;

    [Header("StreetView Camera Settings")]
    [Range(1f, 10f)]
    public float sensitivity = 3.5f;
    private float X;
    private float Y;

    [Header("Rotating Camera Settings")]
    [SerializeField] public Transform player;
    [SerializeField] private Transform castle;
    [SerializeField] private float defaultRadius = 10f;
    [SerializeField] private float smoothTime = .5f;
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
    private float FOVvelocity;

    private float lerpTime = 0;
	
	void LateUpdate () {
        if(currentCameraType == CameraType.Rotating && player != null) {
            UpdateRotatingCamera();
        } else {
            UpdateStreetViewCamera();
        }
	}

    void UpdateRotatingCamera() {
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

        gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothDamp(currentFOV, targetFOV, ref FOVvelocity, smoothTime);
        

        float playerDistance = Vector3.Distance(castle.position, player.position);

        //camera position modifiers
        float distanceScale;
        float cameraHeight = heightCurve.Evaluate(playerDistance);
        
        

        diameter = defaultRadius;

        if (playerDistance > playerBounds) {
            float k = Mathf.Lerp(0, 12, 1 - playerBounds/playerDistance);
            diameter = defaultRadius + k;
        }

        distanceScale = diameter / playerDistance;

        Vector3 newPosition = new Vector3(distanceScale * player.position.x,
                                          cameraHeight, 
                                          distanceScale * player.position.z);
        Vector3 cameraPos = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        transform.position = cameraPos;
        transform.LookAt(player);

    }

    void UpdateStreetViewCamera() {
        if(Input.GetMouseButton(2)) {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * sensitivity, -Input.GetAxis("Mouse X") * sensitivity, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }
    }
}
