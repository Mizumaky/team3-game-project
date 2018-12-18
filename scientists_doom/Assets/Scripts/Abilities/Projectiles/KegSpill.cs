using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegSpill : MonoBehaviour
{
  private int enemyLayer;

  private void Start()
  {
    enemyLayer = LayerMask.NameToLayer("Enemy");
  }

  private void OnDestroy()
  {
    float radius = 3;
    Collider[] hits = Physics.OverlapSphere(transform.position, radius, 1 << enemyLayer);

    foreach (Collider hit in hits)
    {
      hit.GetComponent<EnemyControls>().Slow();
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == enemyLayer)
    {
      Debug.Log("Slowing!");
      other.GetComponent<EnemyControls>().Slow();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.layer == enemyLayer)
    {
      Debug.Log("Ending slow!");
      other.GetComponent<EnemyControls>().RemoveSlow();
    }
  }
}