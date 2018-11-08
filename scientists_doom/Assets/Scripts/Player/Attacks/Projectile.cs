using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

  public Transform casterTransform;
  public float lifetime = 5f;
  private float projectileFlightHeight = 1;

  void Start () {
    StartCoroutine (AutodestroyCountdown ());
  }

  void Update () {
    Ray rayDown;
    rayDown = new Ray (transform.position, Vector3.down);
    RaycastHit hit;
    if (Physics.Raycast (rayDown, out hit, 100f)) {
      if (hit.collider.gameObject.layer == 9) {
        transform.position = new Vector3 (transform.position.x, projectileFlightHeight + hit.point.y, transform.position.z);
      }
    }
  }

  private void OnCollisionEnter (Collision other) {
    if (other.gameObject.layer == 10) { // enemy layer
      Destroy (gameObject);
    }
  }

  IEnumerator AutodestroyCountdown () {
    yield return new WaitForSeconds (lifetime);
    Destroy (gameObject);
    yield return null;
  }
}