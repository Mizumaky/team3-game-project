using System.Collections;
using UnityEngine;

public class BarbarianKegThrowAbility : Ability
{
  [Header("Key")]
  public KeyCode keyCode = KeyCode.Q;

  [Header("Parameters From Ability Data")]
  public int dmgOnSplash;
  public float spillDecayTime;
  public GameObject kegPrefab;
  public GameObject spillPrefab;

  [Header("Parameters")]
  public float noFlightDist;
  public float launchAngle;
  public AnimationCurve travelHeightPerc;
  public Transform spawnPtTS;

  private void Update()
  {
    if (Input.GetKeyDown(keyCode))
    {
      GameObject newKeg = NewKegProjectile();

      Vector3 targetPos = PlayerMovement.GetGroundPosAtMouse();
      float mouseDistFromChar = Vector3.Distance(transform.position, targetPos);
      if (mouseDistFromChar < noFlightDist)
      {
        Drop(newKeg);
      }
      else
      {
        Throw(newKeg, targetPos);
      }
    }
  }

  private GameObject NewKegProjectile()
  {
    GameObject newKeg = Instantiate(kegPrefab, spawnPtTS.position, Quaternion.identity, null);
    BeerKegProjectile proj = newKeg.GetComponent<BeerKegProjectile>();

    if (proj == null)
    {
      Debug.LogWarning("No BeerKegProjectile script present on newKeg!");
      return null;
    }

    proj.Set(dmgOnSplash, spillDecayTime, spillPrefab, transform);
    return newKeg;
  }

  private void Drop(GameObject newKeg)
  {
    Ray rayDown = new Ray(transform.position, Vector3.down);
    RaycastHit hit;

    int groundMask = 1 << LayerMask.NameToLayer("Ground");
    if (Physics.Raycast(rayDown, out hit, 3f, groundMask))
    {
      Destroy(newKeg.GetComponent<Rigidbody>());
      newKeg.transform.position = hit.point;
      return;
    }
    else
    {
      Debug.LogWarning("Failed to place newKeg... Destroying!");
      Destroy(newKeg);
    }
  }

  private void Throw(GameObject newKeg, Vector3 targetPos)
  {
    // Set rigidbody parameters and calculate velocity at the given target
    Rigidbody kegBody = newKeg.GetComponent<Rigidbody>();

    Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
    Vector3 targetXZPos = new Vector3(targetPos.x, 0.0f, targetPos.z);

    // shorthands for the formula
    float R = Vector3.Distance(projectileXZPos, targetXZPos);
    float G = Physics.gravity.y;
    float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
    float H = targetPos.y - transform.position.y;

    // calculate the local space components of the velocity 
    // required to land the projectile on the target object 
    float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
    float Vy = tanAlpha * Vz;

    // create the velocity vector in local space and get it in global space
    Vector3 localVelocity = new Vector3(0f, Vy, Vz);
    Vector3 globalVelocity = transform.TransformDirection(localVelocity);

    // launch the object by setting its initial velocity and flipping its state
    kegBody.velocity = globalVelocity;
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is KegThrowRankData)
    {
      KegThrowRankData data = ((KegThrowRankData)abilityRankData[(int)rank]);
      dmgOnSplash = data.dmgOnSplash;
      spillDecayTime = data.spillDecayTime;
      kegPrefab = data.kegPrefab;
      spillPrefab = data.spillPrefab;
    }
    else
    {
      Debug.LogWarning("BarbarianKegThrowAbility: Invalid ability data!");
    }
  }
}