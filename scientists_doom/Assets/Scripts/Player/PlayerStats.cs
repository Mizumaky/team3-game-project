using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : Stats
{

  [Header("Hero Level")]
  [SerializeField] private int maxHeroLevel = 60;
  [SerializeField] private int heroLevel;
  [SerializeField] private float experience;
  [SerializeField] private float nextLvlExperience;

  #region GettersSetters
  public int GetMaxHeroLevel()
  {
    return maxHeroLevel;
  }

  public int GetCurrentHeroLevel()
  {
    return heroLevel;
  }

  public void LevelUp(int levels)
  {
    heroLevel += levels;
    if (heroLevel > 60)
    {
      heroLevel = 60;
    }
    UpdateStats();
  }

  public void LevelDown(int levels)
  {
    heroLevel -= levels;
    if (heroLevel < 0)
    {
      heroLevel = 0;
    }
    UpdateStats();
  }

  public float GetCurrentHeroExperience()
  {
    return experience;
  }

  public float GetNextLevelExperience()
  {
    return nextLvlExperience;
  }
  #endregion

  private void Awake()
  {
    if (PlayerPrefs.HasKey("heroLevel"))
    {
      heroLevel = PlayerPrefs.GetInt("heroLevel", 0);
    }
    else
    {
      heroLevel = 1;
    }

    if (PlayerPrefs.HasKey("experience"))
    {
      experience = PlayerPrefs.GetFloat("experience", 0);
    }
    else
    {
      experience = 0;
    }

    UpdateStats();
  }

  protected override void UpdateStats()
  {
    _isAlive = true;
    totalMaxHealth = baseMaxHealth + heroLevel * healthIncrement;
    totalAttackDamage = baseAttackDamage + heroLevel * attackDamageIncrement;

    currentHealth = totalMaxHealth;
    EventManager.TriggerEvent("updateHUD");
  }

  private void OnDestroy()
  {
    SaveStatsToPPrefs();
  }

  private void SaveStatsToPPrefs()
  {
    PlayerPrefs.SetInt("heroLevel", heroLevel);
    PlayerPrefs.SetFloat("experience", experience);
  }

}