using UnityEngine;

public class HuntressQuickShotAbility : Ability
{
  #region Variables

  [Header("Key")]
  public KeyCode keyCode = KeyCode.Space;

  [Header("Objects")]
  public GameObject arrowPrefab;
  public GameObject empoweredArrowPrefab;

  [Header("Collision")]
  public LayerMask collisionMask;

  [Header("Travel")]
  public float velocityMagnitude;
  public float travelHeight = 0.5f;
  public Transform arrowSpawnTransform;

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }
  private int _damageEmpowered;
  public int damageEmpowered { get { return _damageEmpowered; } }
  private int _enemiesToHit;
  public int enemiesToHit { get { return _enemiesToHit; } }
  [Space]

  [Header("Passive")]
  private int _stacks = 1;
  public int stacks { get { return _stacks; } }
  private int stacksRequirement;

  #endregion

  private void Start()
  {
    _stacks = 1;
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode))
    {
      Fire();
    }
  }

  private void Fire()
  {
    GameObject prefab = arrowPrefab;

    // Manage stacks and empowerment
    bool isEmpoweredShot = (stacks == stacksRequirement);
    if (isEmpoweredShot)
    {
      _stacks = 0;
      prefab = empoweredArrowPrefab;
    }

    _stacks++;
    EventManager.TriggerEvent("updateCharSpec");

    // Release
    GameObject newArrow = Instantiate(prefab, arrowSpawnTransform.position, Quaternion.identity, null);
    ArrowProjectile proj = newArrow.GetComponent<ArrowProjectile>();

    newArrow.GetComponent<Rigidbody>().velocity = transform.forward * velocityMagnitude;
    proj.SetAndRelease(damage, damageEmpowered, enemiesToHit, transform, travelHeight, isEmpoweredShot, collisionMask);
  }

  public override void UpdateAbilityData()
  {
    if (abilityRankData[(int)rank] is QuickShotRankData)
    {
      QuickShotRankData data = ((QuickShotRankData)abilityRankData[(int)rank]);
      _damage = data.damage;
      _damageEmpowered = data.damageEmpowered;
    }
    else
    {
      Debug.LogWarning("HuntressQuickShotAbility: Invalid ability data!");
    }
  }
}