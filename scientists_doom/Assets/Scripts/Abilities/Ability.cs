using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AbilityManager))]
public class Ability : MonoBehaviour
{
  public enum Rank { Basic, Apprentice, Master }

  [Header("Rank Data Refs")]
  public AbilityRankData[] abilityRankData;
  [Header("Key")]
  public KeyCode keyCode;

  [Header("Rank Data")]
  protected string abilityName;
  protected Sprite icon;
  protected Rank rank = Rank.Basic;
  protected float _cooldown;
  public float cooldown { get { return _cooldown; } }
  protected float _cooldownCountdown;
  public float cooldownCountdown { get { return _cooldownCountdown; } }

  protected bool onCooldown = false;

  public AbilityRankData GetRankData()
  {
    return abilityRankData[(int)rank];
  }

  public AbilityRankData GetNextRankData()
  {
    return abilityRankData[(int)rank + 1];
  }
  public virtual void SetRank(Rank newRank)
  {
    rank = newRank;
    UpdateAbilityData();
  }

  public void IncreaseRank()
  {
    rank++;
    UpdateAbilityData();
  }

  public virtual void UpdateAbilityData()
  {
    abilityName = abilityRankData[(int)rank].abilityName;
    icon = abilityRankData[(int)rank].icon;
    _cooldown = abilityRankData[(int)rank].cooldown;
  }

  protected IEnumerator CooldownRoutine()
  {
    _cooldownCountdown = cooldown;
    onCooldown = true;
    while (cooldownCountdown > 0)
    {
      yield return null;
      _cooldownCountdown -= Time.deltaTime;
    }
    onCooldown = false;
  }
}