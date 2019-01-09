using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HuntressDashAbility : Ability
{
  #region Variables

  [Header("Visuals")]
  public GameObject smokePrefab;

  [Header("Scriptable Parameters")]
  private float _distance;
  public float distance { get { return _distance; } }

  private NavMeshAgent agent;

  #endregion

  private void Awake()
  {
    Init();
  }

  private void Init()
  {
    agent = GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode) && !onCooldown)
    {
      Dash();
    }
  }

  private void Dash()
  {
    Vector3 targetPosition = PlayerMovement.GetGroundPosAtMouse();
    Vector3 direction = (targetPosition - transform.position).normalized;

    Vector3 rayOrigin = transform.position + direction * distance + Vector3.up;
    Ray rayDown = new Ray(rayOrigin, Vector3.down);
    RaycastHit hit;
    float rayLength = 5f;
    int groundMask = 1 << LayerMask.NameToLayer("Ground");

    GameObject smokeTrail, smokeSelf;
    smokeTrail = smokeSelf = null;
    if (Physics.Raycast(rayDown, out hit, rayLength, groundMask))
    {
      smokeTrail = Instantiate(smokePrefab, transform.position, Quaternion.identity, null);
      agent.Warp(hit.point);
      smokeSelf = Instantiate(smokePrefab, transform.position, Quaternion.identity, transform);
      StartCoroutine(CooldownRoutine());
    }
    else
    {
      Debug.LogWarning("Failed to find a place to put ass!");
    }

    if (smokeTrail != null)
    {
      Destroy(smokeTrail, 2);
    }

    if (smokeSelf != null)
    {
      Destroy(smokeSelf, 2);
    }
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is DashRankData)
    {
      DashRankData data = ((DashRankData)abilityRankData[(int)rank]);
      _distance = data.distance;
    }
    else
    {
      Debug.LogWarning("HuntressDashAbility: Invalid ability data!");
    }
  }
}