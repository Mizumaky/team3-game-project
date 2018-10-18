using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_RotatingCamera : NetworkManager {

    [Header("CameraSettings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform rotateAround;
    [SerializeField] private float cameraRadius;
    [SerializeField] private float cameraHeight;
    [SerializeField] private float cameraSpeed;

    private float angle;
    private float velocity;

    private bool canRotate = true;

    public override void OnStartClient(NetworkClient client)
    {
        canRotate = false;
    }

    public override void OnStopClient()
    {
        canRotate = true;
    }

    public override void OnStartHost()
    {
        canRotate = false;
    }

    public override void OnStopHost()
    {
        canRotate = true;
    }

    private void LateUpdate()
    {
        if (!canRotate) {
            return;
        }

        angle += cameraSpeed * Time.deltaTime;
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        Vector3 targetPosition = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * cameraRadius,
                                             cameraHeight,
                                             Mathf.Cos(angle * Mathf.Deg2Rad) * cameraRadius);

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.LookAt(rotateAround);
    }
}
