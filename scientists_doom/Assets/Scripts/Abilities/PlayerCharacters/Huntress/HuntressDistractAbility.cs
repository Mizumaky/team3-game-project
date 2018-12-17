using UnityEngine;

public class HuntressDistractAbility : Ability
{
  #region Variables

  [Header("Key")]
  public KeyCode keyCode = KeyCode.E;

  [Header("Object")]
  public GameObject owlPrefab;
  public Transform owlSpawnTransform;
  public float owlDestinationHeight;

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
    if (Input.GetKeyDown(keyCode))
    {
      SendAtMousePosition();
    }
  }

  private void SendAtMousePosition()
  {
    GameObject newOwl = Instantiate(owlPrefab, owlSpawnTransform.position, Quaternion.identity, null);

    Vector3 groundPosAtMouse = PlayerMovement.GetGroundPosAtMouse();
    Vector3 destination = groundPosAtMouse + Vector3.up * owlDestinationHeight;

    OwlProjectile proj = newOwl.GetComponent<OwlProjectile>();
    proj.SetAndRelease(destination, groundPosAtMouse, duration, transform, areaOutlinePrefab);
  }
}