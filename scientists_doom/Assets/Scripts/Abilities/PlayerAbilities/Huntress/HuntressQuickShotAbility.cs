using UnityEngine;

[RequireComponent(typeof(Stats))]
public class HuntressQuickShotAbility : Ability
{
  #region Variables

  [Header("Objects")]
  public GameObject arrowPrefab;
  public GameObject empoweredArrowPrefab;

  [Header("Collision")]
  public LayerMask collisionMask;

  [Header("Travel")]
  public float velocityMagnitude;
  public float travelHeight = 1f;
  public float timeToLive = 2;
  public Transform arrowSpawnTransform;

  [Header("Scriptable Parameters")]
  private int _damage;
  public int damage { get { return _damage; } }
  private int _damageEmpowered;
  public int damageEmpowered { get { return _damageEmpowered; } }
  [Space]

  [Header("Passive")]
  private int _stacks = 1;
  public int stacks { get { return _stacks; } }
  private int stacksRequirement = 3;

  private Animator animator;

  #endregion

  private void Start()
  {
    _stacks = 1;
    animator = GetComponent<Animator>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode) && !onCooldown)
    {
      animator.SetTrigger("attackTrigger");
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

    float statDamage = GetComponent<Stats>().GetAttackDamage();
    float totalDamage = statDamage + (isEmpoweredShot ? damageEmpowered : damage);

    newArrow.GetComponent<Rigidbody>().velocity = transform.forward * velocityMagnitude;
    proj.Set(totalDamage, isEmpoweredShot, timeToLive, transform, travelHeight, collisionMask);

    StartCoroutine(CooldownRoutine());
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
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