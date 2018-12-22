using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HuntressDashAbility : Ability
{
  #region Variables

  [Header("Visuals")]
  public ParticleSystem dashPS;

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
    if (Input.GetKeyDown(keyCode))
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

    if (Physics.Raycast(rayDown, out hit, rayLength, groundMask))
    {
      agent.Warp(hit.point);
    }
    else
    {
      Debug.LogWarning("Failed to find a place to put ass!");
    }
  }

  public override void UpdateAbilityData()
  {
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