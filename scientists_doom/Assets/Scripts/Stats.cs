using UnityEngine;
using UnityEngine.UI;
public class Stats : MonoBehaviour {

  protected bool alive;
  [Header ("Health")]
  public float baseMaxHealth = 100f; // To keep health cap, so that hero cannot be overhealed 
  protected float totalMaxHealth;
  protected float currentHealth;
  public float healthIncrement = 10f;

  [Header ("Attack")]
  public float baseAttackDamage = 10f;
  protected float totalAttackDamage;
  public float attackDamageIncrement = 1f;

  public bool isAlive () {
    return alive;
  }
  public float GetCurrentHealth () {
    return currentHealth;
  }

  public float GetAttackDamage () {
    return totalAttackDamage;
  }

  private void Start () {
    totalMaxHealth = baseMaxHealth;
    totalAttackDamage = baseAttackDamage;
  }

  public virtual void TakeDamage (float damage) {
    currentHealth -= damage;
    if (currentHealth <= 0) {
      Die ();
    }
  }

  protected virtual void Die () {
    alive = false;
    Debug.Log (gameObject.name + " died!");
  }
}