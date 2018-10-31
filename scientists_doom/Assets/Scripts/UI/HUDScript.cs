﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

  //[SerializeField] private CharacterManager manager; - made activeCharacterObject static, so this is obsolete
  [SerializeField] private Image healthSlider;
  [SerializeField] private Text healthText;
  [SerializeField] private Image xpSlider;
  private PlayerStats playerStatsReference;
  private GameObject lastActiveCharObject = null;

  private void LateUpdate () {
    if (CharacterManager.activeCharacterObject != null) {
      //get new playerstats component reference only on character change
      if (lastActiveCharObject != CharacterManager.activeCharacterObject) {
        playerStatsReference = CharacterManager.activeCharacterObject.GetComponent<PlayerStats> ();
      }
      lastActiveCharObject = CharacterManager.activeCharacterObject;

      float maxPlayerHealth = playerStatsReference.baseMaxHealth;
      float playerHealth = playerStatsReference.GetCurrentHealth ();

      float playerXp = playerStatsReference.experience;
      float nextLevelXp = playerStatsReference.nextLvlExperience;

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