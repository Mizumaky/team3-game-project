using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDScript : MonoBehaviour {

    //[SerializeField] private CharacterManager manager; - made activeCharacterObject static, so this is obsolete
    [SerializeField] private Image healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Image xpSlider;
    private PlayerStats playerStatsReference;
    private GameObject lastActiveCharObject = null;

    private void LateUpdate()
    {
        if(CharacterManager.activeCharacterObject != null) {
            if (lastActiveCharObject != CharacterManager.activeCharacterObject) { //get new playerstats component reference on character change
                playerStatsReference = CharacterManager.activeCharacterObject.GetComponent<PlayerStats>();
            }
            lastActiveCharObject = CharacterManager.activeCharacterObject;

            float maxPlayerHealth = playerStatsReference.maxHealth;
            float playerHealth = playerStatsReference.health;

            float playerXp = playerStatsReference.experience;
            float nextLevelXp = playerStatsReference.nextLvlExperience;
            
            healthSlider.fillAmount = playerHealth / maxPlayerHealth;
            healthText.text = playerHealth + " / " + maxPlayerHealth;
            if (nextLevelXp != 0 && playerXp != 0)
            {
                xpSlider.fillAmount = nextLevelXp / playerXp;
            }
            else {
                xpSlider.fillAmount = 0;
            }
        } else {
            lastActiveCharObject = null;
        }
    }
}
