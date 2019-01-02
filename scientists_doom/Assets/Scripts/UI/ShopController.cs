using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
  public Button weaponButton;
  public Button[] abilityButtons = new Button[3];
  private WeaponManager weaponManager;
  private AbilityManager abilityManager;
  private PlayerStats playerStats;
  private Inventory playerInventory;

  void OnEnable () {
    if (CharacterManager.activeCharacterObject) {
      weaponManager = FindObjectOfType<WeaponManager> ();
      abilityManager = FindObjectOfType<AbilityManager> ();
      playerStats = CharacterManager.activeCharacterObject.GetComponent<PlayerStats> ();
      playerInventory = CharacterManager.activeCharacterObject.GetComponent<Inventory> ();
      Debug.Log ("Shop Opened, Customer: " + CharacterManager.activeCharacter + ", Level: " + playerStats.GetCurrentHeroLevel ());
      SetWeaponButton ();
      SetAbilityButtons ();
    } else {
      Debug.Log ("Could not get customer!");
    }
  }

  private void SetWeaponButton () {
    //Add level distribution (atm, a weapon is unlocked on levels 21, 42)
    int customerLevel = playerStats.GetCurrentHeroLevel () / 21;

    Text tempName = weaponButton.transform.FindDeepChild ("Name").gameObject.GetComponent<Text> ();
    Image tempImage = weaponButton.transform.FindDeepChild ("Icon").gameObject.GetComponent<Image> ();
    Text tempText = weaponButton.transform.FindDeepChild ("Price").gameObject.GetComponent<Text> ();

    bool levelReqMet, notMaxed;
    if (weaponManager.activeWeaponIndex < customerLevel) {
      levelReqMet = true;
    } else {
      levelReqMet = false;
    }

    if (weaponManager.activeWeaponIndex < 2) {
      tempName.text = weaponManager.weaponData[weaponManager.activeWeaponIndex + 1].name;
      tempImage.sprite = weaponManager.weaponData[weaponManager.activeWeaponIndex + 1].icon;
      tempText.text = weaponManager.weaponData[weaponManager.activeWeaponIndex + 1].upgradeCost.ToString ();
      notMaxed = true;
    } else {
      tempName.text = "Nothing";
      tempImage.sprite = null;
      tempText.text = "Maxed";
      notMaxed = false;
    }

    if (levelReqMet && notMaxed) {
      weaponButton.interactable = true;
    } else {
      weaponButton.interactable = false;
    }
  }

  private void SetAbilityButtons () {
    Text tempName;
    Image tempImage;
    Text tempText;

    for (int i = 0; i < 3; i++) {
      tempName = abilityButtons[i].transform.FindDeepChild ("Name").gameObject.GetComponent<Text> ();
      tempImage = abilityButtons[i].transform.FindDeepChild ("Icon").gameObject.GetComponent<Image> ();
      tempText = abilityButtons[i].transform.FindDeepChild ("Price").gameObject.GetComponent<Text> ();

      bool notMaxed;

      if ((int) abilityManager.abilityRanks[i] < 2) {
        AbilityRankData data = abilityManager.abilities[i].GetNextRankData ();
        tempName.text = data.name;
        tempImage.sprite = data.icon;
        tempText.text = data.upgradeCost.ToString ();
        notMaxed = true;
      } else {
        tempName.text = "Nothing!";
        tempImage.sprite = null;
        tempText.text = "Maxed";
        notMaxed = false;
      }

      if (notMaxed) {
        abilityButtons[i].interactable = true;
      } else {
        abilityButtons[i].interactable = false;
      }
    }
  }

  public void BuyWeaponUpgrade () {

    int cost = weaponManager.weaponData[weaponManager.activeWeaponIndex + 1].upgradeCost;
    if (playerInventory.souls >= cost) {
      if (weaponManager.EquipWeapon (weaponManager.activeWeaponIndex + 1)) {
        weaponManager.activeWeaponIndex++;
        playerInventory.TakeSouls (cost);
        SetWeaponButton ();
        Debug.Log ("Weapon upgraded and equipped!");
      } else {
        Debug.Log ("Failed to equip weapon!");
      }
    } else {
      Debug.Log ("Not enough souls!");
    }
  }

  public void BuyAbility (int type) {
    int curRank = abilityManager.GetAbilityRank ((AbilityManager.AbilityTypes) type);

    int cost = abilityManager.abilities[(int) type].GetNextRankData ().upgradeCost;
    if (playerInventory.souls >= cost) {
      abilityManager.IncreaseAbilityRank ((AbilityManager.AbilityTypes) type);
      playerInventory.TakeSouls (cost);
      SetAbilityButtons ();
      Debug.Log (abilityManager.abilities[(int) type] + " increased to " + abilityManager.GetAbilityRank ((AbilityManager.AbilityTypes) type));
    } else {
      Debug.Log ("Not enough souls!");
    }

  }
}