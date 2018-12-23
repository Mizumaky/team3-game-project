using UnityEngine;

public class FreezeChargeup : MonoBehaviour
{
  #region Variables

  [Header("Scriptable Parameters")]
  private float _damage;
  private int _angle;
  private float _stunDuration;
  private float _radius;
  private Transform _casterTransform;
  private LayerMask _collisionMask;

  #endregion

  public void Set(float damage, float radius, int angle, float stunDuration, Transform casterTransform, LayerMask collisionMask)
  {
    this._damage = damage;
    this._radius = radius;
    this._angle = angle;
    this._stunDuration = stunDuration;
    this._casterTransform = casterTransform;
    this._collisionMask = collisionMask;
  }
}