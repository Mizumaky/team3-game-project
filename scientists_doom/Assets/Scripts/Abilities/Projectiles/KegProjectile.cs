using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegProjectile : MonoBehaviour
{
  [Header("Travel")]
  public int damageOnSplash;
  public float spillDecayTime;
  public LayerMask collisionMask;
  public GameObject spillPrefab;

  [Header("Carry")]
  public Transform casterTransform;

  [Header("Other")]
  public GameObject model;
  public ParticleSystem splashPS;

  private Vector3 randomRotVect;
  private bool inFlight;

  public void Fly()
  {
    randomRotVect = new Vector3(2, 1, 0);
    inFlight = true;
    StartCoroutine(Rotation());
  }

  private IEnumerator Rotation()
  {
    while (inFlight)
    {
      transform.Rotate(randomRotVect);
      yield return null;
    }
  }

  public void Set(int dmg, float time, GameObject pfb, Transform ct)
  {
    damageOnSplash = dmg;
    spillDecayTime = time;
    spillPrefab = pfb;
    casterTransform = ct;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (UnityExtensions.ContainsLayer(collisionMask, other.gameObject.layer))
    {
      inFlight = false;

      Destroy(GetComponent<Rigidbody>());
      model.SetActive(false);

      splashPS.Play();
      Spill();
    }
  }

  private void Spill()
  {
    Ray rayDown = new Ray(transform.position + Vector3.up, Vector3.down);
    RaycastHit hit;

    Vector3 groundNormal = Vector3.up;
    Vector3 spillPosition = transform.position;
    int groundMask = 1 << LayerMask.NameToLayer("Ground");
    if (Physics.Raycast(rayDown, out hit, 3f, groundMask))
    {
      GameObject spillObj = Instantiate(spillPrefab, hit.point, Quaternion.LookRotation(hit.normal), transform);
    }

    HitEnemies();

    Destroy(gameObject, spillDecayTime);
  }

  private void HitEnemies()
  {
    float radius = 3;
    int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

    foreach (Collider hit in hits)
    {
      hit.GetComponent<Stats>().TakeDamage(damageOnSplash);
      hit.GetComponent<EnemyControls>().Aggro(casterTransform);
    }
  }

}