using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : Stats
{
  public override void TakeDamage(float damage)
  {
    base.TakeDamage(damage);
    GetComponent<EnemyHealthBar>().AdjustHealthBar(currentHealth / totalMaxHealth);
    GetComponent<EnemyHealthBar>().ShowDmgText(damage);
    if(GetComponent<SpeechBubbleInterface>() != null)
      GetComponent<SpeechBubbleInterface>().SaySomething(SpeechContainer.Mood.Angry, 3f, 0.01f);

  }

  protected override void Die()
  {
    if (_isAlive)
    {
      GetComponent<EnemyControls>().Disable();
      EnemySpawner.enemiesAlive--;
      Debug.Log("Enemies alive: "+EnemySpawner.enemiesAlive);
      if(EnemySpawner.enemiesAlive == 0){
        EventManager.TriggerEvent(LevelManager.EVENT_LEVEL_ENDED);
      }
    }
    base.Die();
    
  }
}