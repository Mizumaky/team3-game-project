using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HUDScript : MonoBehaviour
{
  [SerializeField] private Stats castleStats;
  [SerializeField] private Image castleHealthSlider;
  [SerializeField] private Image healthSlider;
  [SerializeField] private Text healthText;
  [SerializeField] private Image xpSlider;
  [SerializeField] private Text woodText;
  [SerializeField] private Text stoneText;
  [SerializeField] private Text soulsText;

  public GameObject barbarianRageBar;
  public GameObject wizardCastBar;
  public GameObject huntressStackBar;

  public Image[] cooldownImages;

  private AbilityManager abilityManager;
  private PlayerStats playerStatsReference;
  private GameObject lastActiveCharObject = null;

  private void Start()
  {
    EventManager.StartListening("updateHUD", UpdateHUD);
    EventManager.StartListening("changeCharSpec", ChangeCharSpecUI);
    EventManager.StartListening("updateCharSpec", UpdateCharSpecUI);

    UpdateHUD();
  }

  private void LateUpdate()
  {
    abilityManager = CharacterManager.activeCharacterObject.GetComponent<AbilityManager>();
    for (int i = 0; i < abilityManager.abilities.Length; i++)
    {
      cooldownImages[i].fillAmount = abilityManager.abilities[i].cooldownCountdown / abilityManager.abilities[i].cooldown;
    }
  }

  // TODO: This can be made much faster (e.g. listener on character stats)
  private void UpdateHUD()
  {
    if (castleStats)
    {
      float maxCastleHealth = castleStats.GetTotalMaxHealth();
      float castleHealth = castleStats.GetCurrentHealth();

      castleHealthSlider.fillAmount = castleHealth / maxCastleHealth;
    }

    if (CharacterManager.activeCharacterObject != null)
    {
      //get new playerstats component reference only on character change
      if (lastActiveCharObject != CharacterManager.activeCharacterObject)
      {
        playerStatsReference = CharacterManager.activeCharacterObject.GetComponent<PlayerStats>();
      }
      lastActiveCharObject = CharacterManager.activeCharacterObject;

      woodText.text = lastActiveCharObject.GetComponentInChildren<Inventory>().wood.ToString();
      stoneText.text = lastActiveCharObject.GetComponentInChildren<Inventory>().stone.ToString();
      soulsText.text = lastActiveCharObject.GetComponentInChildren<Inventory>().souls.ToString();

      float maxPlayerHealth = playerStatsReference.GetTotalMaxHealth();
      float playerHealth = playerStatsReference.GetCurrentHealth();

      float playerXp = playerStatsReference.GetCurrentHeroExperience();
      float nextLevelXp = playerStatsReference.GetNextLevelExperience();

      healthSlider.fillAmount = playerHealth / maxPlayerHealth;
      healthText.text = playerHealth + " / " + maxPlayerHealth;
      if (nextLevelXp != 0 && playerXp != 0)
      {
        xpSlider.fillAmount = nextLevelXp / playerXp;
      }
      else
      {
        xpSlider.fillAmount = 0;
      }
    }
    else
    {
      lastActiveCharObject = null;
    }
  }

  public void ChangeCharSpecUI()
  {
    switch (CharacterManager.activeCharacter)
    {
      case CharacterManager.Character.Barbarian:
        {
          barbarianRageBar.SetActive(true);
          wizardCastBar.SetActive(false);
          huntressStackBar.SetActive(false);
          break;
        }
      case CharacterManager.Character.Wizard:
        {
          barbarianRageBar.SetActive(false);
          wizardCastBar.SetActive(true);
          huntressStackBar.SetActive(false);
          break;
        }
      case CharacterManager.Character.Huntress:
        {
          barbarianRageBar.SetActive(false);
          wizardCastBar.SetActive(false);
          huntressStackBar.SetActive(true);
          break;
        }
    }
  }

  public void UpdateCharSpecUI()
  {
    switch (CharacterManager.activeCharacter)
    {
      case CharacterManager.Character.Barbarian:
        {
          BarbarianImmortalityAbility ability = CharacterManager.activeCharacterObject.GetComponent<BarbarianImmortalityAbility>();
          if (ability == null)
          {
            Debug.LogWarning("Barbarian rage passive now found on active character!");
          }
          if (ability.stacksCap <= 0) Debug.LogWarning("Barbarian rage stacks cap is 0 or below!");
          else
          {
            float fillAmout = (float)ability.stacks / (float)ability.stacksCap;
            barbarianRageBar.transform.GetChild(0).GetComponent<Image>().fillAmount = fillAmout;
          }
          break;
        }
      case CharacterManager.Character.Wizard:
        {
          WizardChargePassiveAbility ability = CharacterManager.activeCharacterObject.GetComponent<WizardChargePassiveAbility>();
          if (ability == null)
          {
            Debug.LogWarning("Wizard charge passive not found on active character!");
          }
          wizardCastBar.transform.GetChild(0).GetComponent<Image>().fillAmount = ability.chargeProgress;
          break;
        }
      case CharacterManager.Character.Huntress:
        {
          HuntressQuickShotAbility ability = CharacterManager.activeCharacterObject.GetComponent<HuntressQuickShotAbility>();
          if (ability == null)
          {
            Debug.LogWarning("Huntress quick shot ability not found on active character!");
          }

          Animator anim = huntressStackBar.GetComponent<Animator>();
          anim.SetInteger("stacks", ability.stacks - 1);

          break;
        }
    }
  }

  public void UpdateCooldowns()
  {

  }
}