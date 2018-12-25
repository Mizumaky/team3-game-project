using UnityEngine;
using UnityEngine.UI;
public class Stats : MonoBehaviour
{
  [Header("Health")]
  public float baseMaxHealth = 100f; // To keep health cap, so that hero cannot be overhealed 
  [SerializeField] protected float totalMaxHealth;
  [SerializeField] protected float currentHealth;
  public float healthIncrement = 10f;

  protected bool _isAlive;
  public bool isAlive { get { return _isAlive; } }

  [Header("Attack")]
  public float baseAttackDamage = 10f;
  [SerializeField] protected float totalAttackDamage;
  public float attackDamageIncrement = 1f;
  public bool isInvulnerable = false;

  public float GetTotalMaxHealth()
  {
    return totalMaxHealth;
  }
  public float GetCurrentHealth()
  {
    return currentHealth;
  }

  public float GetAttackDamage()
  {
    return totalAttackDamage;
  }

  private void Start()
  {
    UpdateStats();
  }

  public void AddBonusDamage(int amount)
  {
    totalAttackDamage += amount;
  }

  public void RemoveBonusDamage(int amount)
  {
    totalAttackDamage -= amount;
  }

  public void ResetBonusDamage()
  {
    totalAttackDamage = baseAttackDamage;
  }

  protected virtual void UpdateStats()
  {
    _isAlive = true;

    totalMaxHealth = baseMaxHealth;
    totalAttackDamage = baseAttackDamage;

    currentHealth = totalMaxHealth;
  }

  public virtual void TakeDamage(float damage)
  {
    if (!isInvulnerable)
    {
      currentHealth -= damage;
      if (currentHealth <= 0)
      {
        Die();
      }
    }
  }

  protected virtual void Die()
  {
    if (_isAlive)
    {
      _isAlive = false;
      Debug.Log(gameObject.name + " died!");
    }
  }
}