using System.Collections;
using UnityEngine;

public class BarbarianKegThrowAbility : Ability
{
  #region Variables

  [Header("Object")]
  public GameObject kegPrefab;
  public GameObject spillPrefab;

  [Header("Collision")]
  public LayerMask collisionMask;

  [Header("Travel")]
  public float launchAngle;
  public float noFlightDist;
  public Transform kegSpawnTransform;

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }
  private float _spillDuration;
  public float spillDuration { get { return _spillDuration; } }

  #endregion

  private void Update()
  {
    if (Input.GetKeyDown(keyCode))
    {
      Vector3 targetPosition = PlayerMovement.GetGroundPosAtMouse();

      float mouseDistFromChar = Vector3.Distance(transform.position, targetPosition);
      if (mouseDistFromChar < noFlightDist)
      {
        Drop();
      }
      else
      {
        Throw(targetPosition);
      }
    }
  }

  private void Drop()
  {
    Ray rayDown = new Ray(transform.position, Vector3.down);
    RaycastHit hit;
    float rayLength = 3f;
    int groundMask = 1 << LayerMask.NameToLayer("Ground");

    if (Physics.Raycast(rayDown, out hit, rayLength, groundMask))
    {
      GameObject newKeg = Instantiate(kegPrefab, hit.point, Quaternion.identity, null);
      Destroy(newKeg.GetComponent<Rigidbody>());
    }
    else
    {
      Debug.LogWarning("Failed to place newKeg!");
    }
  }

  private void Throw(Vector3 targetPosition)
  {
    // Release
    GameObject newKeg = Instantiate(kegPrefab, kegSpawnTransform.position, Quaternion.identity, null);
    KegProjectile proj = newKeg.GetComponent<KegProjectile>();

    SetVelocityTowardsTargetPosition(newKeg, targetPosition);

    proj.SetAndRelease(damage, spillDuration, spillPrefab, transform, collisionMask);
  }

  private void SetVelocityTowardsTargetPosition(GameObject newKeg, Vector3 targetPosition)
  {
    // Set rigidbody parameters and calculate velocity at the given target
    Rigidbody kegRigidbody = newKeg.GetComponent<Rigidbody>();

    Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
    Vector3 targetXZPos = new Vector3(targetPosition.x, 0.0f, targetPosition.z);

    // shorthands for the formula
    float R = Vector3.Distance(projectileXZPos, targetXZPos);
    float G = Physics.gravity.y;
    float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
    float H = targetPosition.y - transform.position.y;

    // calculate the local space components of the velocity 
    // required to land the projectile on the target object 
    float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
    float Vy = tanAlpha * Vz;

    // create the velocity vector in local space and get it in global space
    Vector3 localVelocity = new Vector3(0f, Vy, Vz);
    Vector3 globalVelocity = transform.TransformDirection(localVelocity);

    // launch the object by setting its initial velocity and flipping its state
    kegRigidbody.velocity = globalVelocity;
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is KegThrowRankData)
    {
      KegThrowRankData data = ((KegThrowRankData)abilityRankData[(int)rank]);
      _damage = data.damage;
      _spillDuration = data.spillDuration;
    }
    else
    {
      Debug.LogWarning("BarbarianKegThrowAbility: Invalid ability data!");
    }
  }
}