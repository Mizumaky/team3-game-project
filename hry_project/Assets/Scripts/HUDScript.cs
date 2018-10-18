using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDScript : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private Image healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image xpSlider;

    private void LateUpdate()
    {
        int maxPlayerHealth = (int)player.GetComponent<PlayerStats>().maxHealth;
        int playerHealth = (int)player.GetComponent<PlayerStats>().health;

        int playerXp = (int)player.GetComponent<PlayerStats>().experience;
        int nextLevelXp = (int)player.GetComponent<PlayerStats>().nextLvlExperience;

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
