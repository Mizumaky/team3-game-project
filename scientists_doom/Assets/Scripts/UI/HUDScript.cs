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
        int maxPlayerHealth = (int)manager.activeCharacter.GetComponent<PlayerStats>().maxHealth;
        int playerHealth = (int)manager.activeCharacter.GetComponent<PlayerStats>().health;

        int playerXp = (int)manager.activeCharacter.GetComponent<PlayerStats>().experience;
        int nextLevelXp = (int)manager.activeCharacter.GetComponent<PlayerStats>().nextLvlExperience;

        healthSlider.fillAmount = maxPlayerHealth / playerHealth;
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
