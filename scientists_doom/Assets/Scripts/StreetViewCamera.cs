 using UnityEngine;
 
 public class StreetViewCamera : MonoBehaviour {

	 [Range(1f, 10f)]
     public float sensitivity = 3.5f;
     private float X;
     private float Y;
 
     void Update() {
         if(Input.GetMouseButton(2)) {
             transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * sensitivity, -Input.GetAxis("Mouse X") * sensitivity, 0));
             X = transform.rotation.eulerAngles.x;
             Y = transform.rotation.eulerAngles.y;
             transform.rotation = Quaternion.Euler(X, Y, 0);
         }
     }
 }