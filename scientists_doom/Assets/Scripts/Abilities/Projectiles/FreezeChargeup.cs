using System.Collections;
using UnityEngine;

public class FreezeChargeup : MonoBehaviour
{
  #region Variables
  public GameObject freezeBurstPrefab;

  [Header("Scriptable Parameters")]
  private float _damage;
  private int _angle;
  private float _stunDuration;
  private float _radius;
  private Transform _casterTransform;

  #endregion

  public void Set(float damage, float radius, int angle, float stunDuration, Transform casterTransform)
  {
    this._damage = damage;
    this._radius = radius;
    this._angle = angle;
    this._stunDuration = stunDuration;
    this._casterTransform = casterTransform;

    Release();
  }

  private void Release()
  {
    HitEnemies();

    Ray rayDown = new Ray(transform.position + Vector3.up, Vector3.down);
    RaycastHit hit;

    Vector3 point1 = Vector3.zero, point2 = Vector3.zero;
    if (Physics.Raycast(rayDown, out hit, 20f, 1 << LayerMask.NameToLayer("Ground")))
    {
      point1 = hit.point;

      rayDown = new Ray(transform.position + Vector3.up * 5f + transform.forward * 5f, Vector3.down);
      if (Physics.Raycast(rayDown, out hit, 20f, 1 << LayerMask.NameToLayer("Ground")))
      {
        point2 = hit.point;
      }
    }

    if (point1 != Vector3.zero && point2 != Vector3.zero)
    {
      Vector3 direction = (point1 - point2).normalized;
      GameObject newBurst = Instantiate(freezeBurstPrefab, point1 + Vector3.up, Quaternion.LookRotation(direction), null);
      newBurst.transform.localScale *= _radius;

      ParticleSystem.ShapeModule shape = newBurst.GetComponent<ParticleSystem>().shape;
      shape.rotation = new Vector3(-90, 0, 90 - _angle / 2f);
      shape.arc = _angle;

      newBurst.transform.GetChild(0).localScale *= _radius;

      GetComponent<ParticleSystem>().Stop();
      Destroy(newBurst, 1);
    }
  }

  private void HitEnemies()
  {
    int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    Collider[] hits = Physics.OverlapSphere(transform.position, _radius, enemyLayer);
    Vector3 directionToHit;
    float angle;
    foreach (Collider hit in hits)
    {
      directionToHit = hit.transform.position - transform.position;
      angle = Vector3.Angle(transform.forward, directionToHit);
      if (angle < _angle / 2)
      {
        hit.GetComponent<Stats>().TakeDamage(_damage);
        hit.GetComponent<EnemyControls>().AggroTo(_casterTransform);
        hit.GetComponent<EnemyControls>().Stun();
      }
    }

    StartCoroutine(RemoveStun(hits));
  }

  private IEnumerator RemoveStun(Collider[] hits)
  {
    yield return new WaitForSeconds(_stunDuration);
    foreach (Collider hit in hits)
    {
      if (hit != null)
      {
        hit.GetComponent<EnemyControls>().RemoveStun();
      }
    }
    Destroy(gameObject);
  }
}