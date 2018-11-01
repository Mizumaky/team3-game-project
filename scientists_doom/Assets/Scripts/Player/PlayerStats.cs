using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : Stats {

  [Header ("Hero Level")]
  [SerializeField]
  private int maxHeroLevel = 60;
  [HideInInspector]
  public int heroLevel;
  [HideInInspector]
  public float experience;
  [HideInInspector]
  public float nextLvlExperience;

  private void Awake () {
    if (PlayerPrefs.HasKey ("heroLevel")) {
      heroLevel = PlayerPrefs.GetInt ("heroLevel", 0);
    } else {
      heroLevel = 1;
    }

    if (PlayerPrefs.HasKey ("experience")) {
      experience = PlayerPrefs.GetFloat ("experience", 0);
    } else {
      experience = 0;
    }
  }

  private void Start () {
    Init ();
  }

  protected override void Init () {
    totalMaxHealth = baseMaxHealth + heroLevel * healthIncrement;
    totalAttackDamage = baseAttackDamage + heroLevel * attackDamageIncrement;

    currentHealth = totalMaxHealth;
  }

  private void OnDestroy () {
    SaveStatsToPPrefs ();
  }

  private void SaveStatsToPPrefs () {
    PlayerPrefs.SetInt ("heroLevel", heroLevel);
    PlayerPrefs.SetFloat ("experience", experience);
  }

}