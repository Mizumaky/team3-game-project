using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegSpill : MonoBehaviour
{
  private void OnDestroy()
  {
    float radius = 3;
    int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

    foreach (Collider hit in hits)
    {
      hit.GetComponent<EnemyControls>().isSlowed = false;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    {
      Debug.Log("Ending slow!");
      other.GetComponent<EnemyControls>().isSlowed = false;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    {
      Debug.Log("Slowing!");
      other.GetComponent<EnemyControls>().Slow();
    }
  }

}