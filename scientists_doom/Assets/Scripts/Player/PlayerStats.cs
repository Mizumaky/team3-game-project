using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    [SerializeField]
    private bool playerAlive;
    [Header("Health")]
    public float maxHealth = 100; // To keep health cap, so that hero cannot be overhealed 
    public float health;
    [SerializeField]
    [Tooltip("Per level health increment")]
    private float healthIncrement = 10;

    [Header("Hero Level")]
    [SerializeField]
    private int maxHeroLevel = 60;
    [HideInInspector]
    public int heroLevel;
    [HideInInspector]
    public float experience;
    [HideInInspector]
    public float nextLvlExperience;
    
    private float attackDamage = 10;
    [Header("Attack")]
    [SerializeField]
    [Tooltip("Per level attack increment")]
    private float attackIncrement;

    
    private float armor = 0;
    [Header("Armor")]
    [SerializeField]
    [Tooltip("Per level armor increment")]
    private float armorIncrement;

    private void Start()
    {
        playerAlive = true;

        //Load playerPrefs
        heroLevel = PlayerPrefs.GetInt("heroLevel", 0);
        experience = PlayerPrefs.GetFloat("experience", 0);

        health = maxHealth + heroLevel * healthIncrement;
        maxHealth = health;

        attackDamage += heroLevel * attackIncrement;
        armor += heroLevel * armorIncrement;
    }

    private void OnDestroy()
    {
        saveStats();
    }

    public void saveStats() {

        PlayerPrefs.SetInt("heroLevel", heroLevel);
        PlayerPrefs.SetFloat("experience", experience);

    }

    public void TakeDamage(float damage) {
        health -= damage - armor;
        if (health <= 0) {
            KillPlayer();
        }
    }

    public void KillPlayer() {
        playerAlive = false;
        Debug.Log("Player is dead.");
    }
}
