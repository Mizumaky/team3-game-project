using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : Stats
{

  [Header("Hero Level")]
  [SerializeField] private int maxHeroLevel = 60;
  [SerializeField] private int heroLevel;
  [SerializeField] private int experience;
  [SerializeField] private int nextLvlExperience;
  private int firstLevelExp = 200;
  private float nextLevelExpMultiplier = 1.3f;

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

  public void GainExp(int expAmount){
    if(heroLevel < maxHeroLevel){
      experience += expAmount;
      Debug.Log("Experience: "+experience);
    }
    if(experience >= nextLvlExperience){
      LevelUp(1);
      experience = experience % nextLvlExperience;
      nextLvlExperience = (int)(nextLvlExperience * nextLevelExpMultiplier);
      Debug.Log("Hero Level Up, cur level: "+heroLevel);
    }
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
      experience = PlayerPrefs.GetInt("experience", 0);
      nextLvlExperience = firstLevelExp * (int)Mathf.Pow(nextLevelExpMultiplier, (float)heroLevel+1);
    }
    else
    {
      experience = 0;
      
      nextLvlExperience = firstLevelExp * (int)Mathf.Pow(nextLevelExpMultiplier, (float)heroLevel+1);
    }
    Debug.Log("Set exp to: "+experience+" nextLevelExp: "+nextLvlExperience);

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
    PlayerPrefs.SetInt("experience", experience);
  }

}