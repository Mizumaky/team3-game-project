using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProjectile : CharacterAbility {

  public GameObject explosion;
  private Light light;
	public float lifetime = 5f;
    public float projectileFlightHeight = 1f;

  private void Awake() {
    light = GetComponent<Light>();
  }

	void Start () {
		StartCoroutine(Fade());
	}

    void Update()
    {
        Ray rayDown;
        rayDown = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(rayDown, out hit, 100f))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                transform.position = new Vector3(transform.position.x, projectileFlightHeight + hit.point.y, transform.position.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
    if(other.gameObject.layer == 10) { // enemy layer
      StartCoroutine(Explode());
    }
  }
	
	IEnumerator Fade() {
    float tick = 0.1f;
    float ticks = lifetime / tick;
    float intensityDec = light.intensity / ticks;
    while(light.intensity > 0) {
      yield return new WaitForSeconds(tick);
      light.intensity -= intensityDec;
    }
    Destroy(gameObject);
	}

  private IEnumerator Explode() {
    Destroy(GetComponent<Rigidbody>());
    Instantiate(explosion, transform.position, transform.rotation, transform);
    if(light != null) {
      FadeLight();
    }
    yield return new WaitForSeconds(0.5f);
    Destroy(gameObject);
  }

  private IEnumerator FadeLight() {
    while(light.intensity > 0) {
      yield return new WaitForSeconds(0.1f);
      light.intensity -= 0.1f;
    }
  }
}
