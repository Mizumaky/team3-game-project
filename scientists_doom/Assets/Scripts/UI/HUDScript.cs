using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

  [SerializeField] private Image healthSlider;
  [SerializeField] private Text healthText;
  [SerializeField] private Image xpSlider;
  private PlayerStats playerStatsReference;
  private GameObject lastActiveCharObject = null;

  // TODO: This can be made much faster (e.g. listener on character stats)
  private void LateUpdate () {
    if (CharacterManager.activeCharacterObject != null) {
      //get new playerstats component reference only on character change
      if (lastActiveCharObject != CharacterManager.activeCharacterObject) {
        playerStatsReference = CharacterManager.activeCharacterObject.GetComponent<PlayerStats> ();
      }
      lastActiveCharObject = CharacterManager.activeCharacterObject;

      float maxPlayerHealth = playerStatsReference.GetTotalMaxHealth ();
      float playerHealth = playerStatsReference.GetCurrentHealth ();

      float playerXp = playerStatsReference.GetCurrentHeroExperience ();
      float nextLevelXp = playerStatsReference.GetNextLevelExperience ();

      healthSlider.fillAmount = playerHealth / maxPlayerHealth;
      healthText.text = playerHealth + " / " + maxPlayerHealth;
      if (nextLevelXp != 0 && playerXp != 0) {
        xpSlider.fillAmount = nextLevelXp / playerXp;
      } else {
        xpSlider.fillAmount = 0;
      }
    } else {
      lastActiveCharObject = null;
    }
  }
}