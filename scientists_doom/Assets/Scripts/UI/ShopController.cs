using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
  [Header("Buttons")]
  public Button weaponButton;
  public Button[] abilityButtons = new Button[3];

  [Header("Soul Balance")]
  public Text soulBalanceText;

  private WeaponManager weaponManager;
  private AbilityManager abilityManager;
  private PlayerStats playerStats;
  private Inventory playerInventory;

  private Text wepNameText, wepLvlReqText, wepDmgImpvText, wepPriceText;
  private Image wepIconImage;

  private Text[] abNameText = new Text[3], abDescText = new Text[3], abPriceText = new Text[3];
  private Image[] abIconImage = new Image[3];

  private void Awake()
  {
    Init();
  }

  void OnEnable()
  {
    if (CharacterManager.activeCharacterObject)
    {
      weaponManager = FindObjectOfType<WeaponManager>();
      abilityManager = FindObjectOfType<AbilityManager>();
      playerStats = CharacterManager.activeCharacterObject.GetComponent<PlayerStats>();
      playerInventory = CharacterManager.activeCharacterObject.GetComponent<Inventory>();

      soulBalanceText.text = playerInventory.souls.ToString();

      SetWeaponButton();
      SetAbilityButtons();

      Debug.Log("Shop for " + CharacterManager.activeCharacter + " at level " + playerStats.GetCurrentHeroLevel() + " opened!");
    }
    else
    {
      Debug.LogWarning("No active character found!");
    }
  }

  private void Init()
  {
    wepNameText = weaponButton.transform.FindDeepChild("Name").GetComponent<Text>();
    wepLvlReqText = weaponButton.transform.FindDeepChild("Req").GetComponent<Text>();
    wepDmgImpvText = weaponButton.transform.FindDeepChild("Val").GetComponent<Text>();
    wepIconImage = weaponButton.transform.FindDeepChild("Icon").GetComponent<Image>();
    wepPriceText = weaponButton.transform.FindDeepChild("Price").GetComponent<Text>();

    for (int i = 0; i < abilityButtons.Length; i++)
    {
      abNameText[i] = abilityButtons[i].transform.FindDeepChild("Name").gameObject.GetComponent<Text>();
      abDescText[i] = abilityButtons[i].transform.FindDeepChild("Desc").gameObject.GetComponent<Text>();
      abIconImage[i] = abilityButtons[i].transform.FindDeepChild("Icon").gameObject.GetComponent<Image>();
      abPriceText[i] = abilityButtons[i].transform.FindDeepChild("Price").gameObject.GetComponent<Text>();
    }
  }

  private void SetWeaponButton()
  {
    bool notMaxed = (weaponManager.activeWeaponIndex < weaponManager.weapons.Length - 1),
    levelReqMet = false,
    resourcesAvl = false;

    if (notMaxed)
    {
      WeaponData curWepData = weaponManager.weaponData[weaponManager.activeWeaponIndex];
      WeaponData nextWepData = weaponManager.weaponData[weaponManager.activeWeaponIndex + 1];

      levelReqMet = playerStats.GetCurrentHeroLevel() >= nextWepData.levelReq;
      resourcesAvl = playerInventory.souls >= nextWepData.upgradeCost;

      wepNameText.text = nextWepData.weaponName;
      wepLvlReqText.text = "Level req: " + nextWepData.levelReq.ToString();
      wepDmgImpvText.text = "+" + (nextWepData.damage - curWepData.damage).ToString() + " dmg";
      wepIconImage.sprite = nextWepData.icon;
      wepPriceText.text = "Cost: " + nextWepData.upgradeCost.ToString();
    }
    else
    {
      wepLvlReqText.text = "";
      wepDmgImpvText.text = "";
      wepPriceText.text = "Maxed";
    }

    if (notMaxed && levelReqMet && resourcesAvl)
    {
      weaponButton.interactable = true;
    }
    else
    {
      weaponButton.interactable = false;
    }
  }

  private void SetAbilityButtons()
  {
    AbilityRankData nextRankData;
    for (int i = 0; i < abilityButtons.Length; i++)
    {
      bool notMaxed = ((int)abilityManager.abilityRanks[i] < 2),
      resourcesAvl = false;

      if (notMaxed)
      {
        nextRankData = abilityManager.abilities[i].GetNextRankData();
        resourcesAvl = playerInventory.souls >= nextRankData.upgradeCost;

        abNameText[i].text = nextRankData.abilityName;
        abDescText[i].text = nextRankData.description;
        abIconImage[i].sprite = nextRankData.icon;
        abPriceText[i].text = nextRankData.upgradeCost.ToString();
      }
      else
      {
        abPriceText[i].text = "Maxed";
      }

      if (notMaxed && resourcesAvl)
      {
        abilityButtons[i].interactable = true;
      }
      else
      {
        abilityButtons[i].interactable = false;
      }
    }
  }

  public void BuyWeaponUpgrade()
  {
    WeaponData nextWepData = weaponManager.weaponData[weaponManager.activeWeaponIndex + 1];

    if (weaponManager.EquipWeapon(weaponManager.activeWeaponIndex + 1))
    {
      playerInventory.TakeSouls(nextWepData.upgradeCost);
      soulBalanceText.text = playerInventory.souls.ToString();

      SetWeaponButton();
      SetAbilityButtons();

      Debug.Log("Weapon upgraded and equipped!");
    }
    else
    {
      Debug.Log("Failed to equip weapon!");
    }
  }

  public void BuyAbility(int type)
  {
    abilityManager.IncreaseAbilityRank((AbilityManager.AbilityTypes)type);

    playerInventory.TakeSouls(abilityManager.abilities[(int)type].GetRankData().upgradeCost);
    soulBalanceText.text = playerInventory.souls.ToString();

    SetWeaponButton();
    SetAbilityButtons();

    Debug.Log(abilityManager.abilities[(int)type] + " increased to " + abilityManager.GetAbilityRank((AbilityManager.AbilityTypes)type));
  }

  public void Add100Souls()
  {
    playerInventory.souls += 100;
    soulBalanceText.text = playerInventory.souls.ToString();

    SetWeaponButton();
    SetAbilityButtons();
  }
}