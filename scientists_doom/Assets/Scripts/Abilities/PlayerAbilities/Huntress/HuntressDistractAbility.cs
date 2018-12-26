using UnityEngine;

public class HuntressDistractAbility : Ability
{
  #region Variables

  [Header("Object")]
  public GameObject eaglePrefab;
  public Transform eagleSpawnTransform;
  public float eagleVelocity;
  public float eagleDestinationHeight;

  [Header("Visuals")]
  public GameObject areaOutlinePrefab;

  [Header("Scriptable Parameters")]
  private float _radius;
  public float radius { get { return _radius; } }
  private float _duration;
  public float duration { get { return _duration; } }

  #endregion

  private void Update()
  {
    if (Input.GetKeyDown(keyCode) && !onCooldown)
    {
      SendAtMousePosition();
    }
  }

  private void SendAtMousePosition()
  {
    GameObject newOwl = Instantiate(eaglePrefab, eagleSpawnTransform.position, Quaternion.identity, null);

    Vector3 groundPosAtMouse = PlayerMovement.GetGroundPosAtMouse();
    Vector3 destination = groundPosAtMouse + Vector3.up * eagleDestinationHeight;

    EagleProjectile proj = newOwl.GetComponent<EagleProjectile>();
    proj.SetAndRelease(destination, groundPosAtMouse, eagleVelocity, radius, duration, transform, areaOutlinePrefab);

    StartCoroutine(CooldownRoutine());
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is DistractRankData)
    {
      DistractRankData data = ((DistractRankData)abilityRankData[(int)rank]);
      _radius = data.radius;
      _duration = data.duration;
    }
    else
    {
      Debug.LogWarning("HuntressDistractAbility: Invalid ability data!");
    }
  }
}