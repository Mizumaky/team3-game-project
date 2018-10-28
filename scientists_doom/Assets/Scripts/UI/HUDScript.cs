using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDScript : MonoBehaviour {

    [SerializeField] private CharacterManager manager;
    [SerializeField] private Image healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Image xpSlider;

    private void LateUpdate()
    {
        if(manager.activeCharacter != null) {
            float maxPlayerHealth = manager.activeCharacter.GetComponent<PlayerStats>().maxHealth;
            float playerHealth = manager.activeCharacter.GetComponent<PlayerStats>().health;

            float playerXp = manager.activeCharacter.GetComponent<PlayerStats>().experience;
            float nextLevelXp = manager.activeCharacter.GetComponent<PlayerStats>().nextLvlExperience;
            
            healthSlider.fillAmount = playerHealth / maxPlayerHealth;
            healthText.text = playerHealth + " / " + maxPlayerHealth;
            if (nextLevelXp != 0 && playerXp != 0)
            {
                xpSlider.fillAmount = nextLevelXp / playerXp;
            }
            else {
                xpSlider.fillAmount = 0;
            }
        }
    }
}
