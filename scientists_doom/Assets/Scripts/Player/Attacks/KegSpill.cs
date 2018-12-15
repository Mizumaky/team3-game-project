using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegSpill : MonoBehaviour
{
  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    {
      Debug.Log("Slowing!");
      other.GetComponent<EnemyControls>().SlowFor(1f);
    }
  }

}