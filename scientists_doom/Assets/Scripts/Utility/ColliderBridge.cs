using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
  MeleeAbility _listener;
  public void Initialize(MeleeAbility l)
  {
    _listener = l;
  }
  void OnTriggerEnter(Collider other)
  {
    _listener.OnWeaponTriggerEnter(other);
  }
}