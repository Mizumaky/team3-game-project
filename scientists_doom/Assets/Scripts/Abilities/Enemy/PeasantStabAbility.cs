using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class PeasantStabAbility : MeleeAbility
{
  [Header("Parameters From Ability Data")]
  public int damage = 5;

  private Stats stats;

  private void Awake()
  {
    Init();
  }

  protected override void Init()
  {
    base.Init();

    stats = GetComponent<Stats>();
  }

  protected override void Collide(Collider other)
  {
    float totalDamage = damage + stats.GetAttackDamage();

    if (enableCollision == true)
    {
      int otherLayer = other.gameObject.layer;

      if (otherLayer == LayerMask.NameToLayer("Player") || otherLayer == LayerMask.NameToLayer("Castle"))
      {
        Debug.Log("Hitting " + other.name);
        other.GetComponent<Stats>().TakeDamage(totalDamage);
        EventManager.TriggerEvent("updateHUD");
      }
    }
  }
}