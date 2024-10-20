using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class BarbarianSlashAbility : MeleeAbility
{
  [Header("Parameters From Ability Data")]
  public int damage;

  private Stats stats;
  private BarbarianImmortalityAbility barbarianImmortalityAbility;

  private void Awake()
  {
    Init();
  }

  private void Update()
  {
    if (Input.GetKeyDown(keyCode) && !onCooldown)
    {
      anim.SetTrigger("attackTrigger");
      StartCoroutine(CooldownRoutine());
    }
  }

  protected override void Init()
  {
    base.Init();
    if ((barbarianImmortalityAbility = GetComponent<BarbarianImmortalityAbility>()) == null) Debug.LogWarning("No barbarian rage passive found!");

    stats = GetComponent<Stats>();
  }

  protected override void Collide(Collider other)
  {
    float totalDamage = damage + stats.GetAttackDamage();

    if (enableCollision == true)
    {
      int otherLayer = other.gameObject.layer;

      if (otherLayer == LayerMask.NameToLayer("Enemy") || otherLayer == LayerMask.NameToLayer("Explosive"))
      {
        barbarianImmortalityAbility.IncreaseStacks();
      }

      if (otherLayer == LayerMask.NameToLayer("Enemy"))
      {
        other.GetComponent<EnemyControls>().AggroTo(transform);
        other.GetComponent<Stats>().TakeDamage(totalDamage);
      }
      else if (otherLayer == LayerMask.NameToLayer("Explosive"))
      {
        other.GetComponent<Explosive>().Explode(transform);
      }
    }
  }

  public override void UpdateAbilityData()
  {
    base.UpdateAbilityData();
    if (abilityRankData[(int)rank] is BarbSlashRankData)
    {
      BarbSlashRankData data = ((BarbSlashRankData)abilityRankData[(int)rank]);
      damage = data.damage;
    }
    else
    {
      Debug.LogWarning("BarbarianSlashAbility: Invalid ability data!");
    }
  }
}