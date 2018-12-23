using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	public Customer[] customers;
    private Customer activeCustomer;
    public Button[] weaponButtons;
    public Button[] firstSkillButtons;
    public Button[] secondSkillButtons;
    private WeaponManager weaponManager;
    private AbilityManager abilityManager;
    private PlayerStats playerStats;
    private Inventory playerInventory;

    void OnEnable(){
        
        activeCustomer = customers[(int)CharacterManager.activeCharacter];
        if(activeCustomer){
            weaponManager = FindObjectOfType<WeaponManager>();
            abilityManager = FindObjectOfType<AbilityManager>();
            playerStats = CharacterManager.activeCharacterObject.GetComponent<PlayerStats>();
            playerInventory = CharacterManager.activeCharacterObject.GetComponent<Inventory>();
            Debug.Log("Shop Opened, Customer: "+activeCustomer+", Level: " + playerStats.GetCurrentHeroLevel());
            ShowWeapons(activeCustomer);
            ShowSkills(activeCustomer);
        }
    }

    private void ShowWeapons(Customer customer){
        Image tempImage;
        Text tempText;

        //Add level distribution (atm, a weapon is unlocked on levels 17, 34, 51)
        int customerLevel = playerStats.GetCurrentHeroLevel() / 17;
        for(int i = 0; i < 3; i++){
            tempImage = weaponButtons[i].transform.FindDeepChild("WeaponImage").gameObject.GetComponent<Image>();
            tempText = weaponButtons[i].transform.FindDeepChild("PriceText").gameObject.GetComponent<Text>();
            tempImage.sprite = customer.weapons[i].icon;
            tempText.text = customer.weapons[i].price.ToString();
            if(i <= customerLevel){
                weaponButtons[i].interactable = true;
            }else{
                weaponButtons[i].interactable = false;
            }
        }
    }
    private void ShowSkills(Customer customer){
        Image tempImage;
        Text tempText;  

        //Add level distribution (atm, a weapon is unlocked on levels 17, 34, 51)
        int customerLevel = playerStats.GetCurrentHeroLevel() / 17;
        
        for(int i = 0; i < 3; i++){
            // Populate firstSkill buttons with info
            tempImage = firstSkillButtons[i].transform.FindDeepChild("WeaponImage").gameObject.GetComponent<Image>();
            tempText = firstSkillButtons[i].transform.FindDeepChild("PriceText").gameObject.GetComponent<Text>();
            tempImage.sprite = customer.firstSkill[i].icon;
            tempText.text = customer.secondSkill[i].price.ToString();
            //Populate secondSkill buttons with info
            tempImage = secondSkillButtons[i].transform.FindDeepChild("WeaponImage").gameObject.GetComponent<Image>();
            tempText = secondSkillButtons[i].transform.FindDeepChild("PriceText").gameObject.GetComponent<Text>();
            tempImage.sprite = customer.secondSkill[i].icon;
            tempText.text = customer.firstSkill[i].price.ToString();

            if(i <= customerLevel){
                if(abilityManager.GetAbilityRank(AbilityManager.AbilityTypes.First)+1 == i){
                    firstSkillButtons[i].interactable = true;
                }
                else{
                    firstSkillButtons[i].interactable = false;
                }
                if(abilityManager.GetAbilityRank(AbilityManager.AbilityTypes.Second)+1 == i){
                    secondSkillButtons[i].interactable = true;
                }else{
                    secondSkillButtons[i].interactable = false;
                }
            }else{
                firstSkillButtons[i].interactable = false;
                secondSkillButtons[i].interactable = false;
            }
        }
    }

    public void BuyWeapon(int weaponIndex){
        if(playerInventory.souls >= activeCustomer.weapons[weaponIndex].price){
            if(weaponManager.EquipWeapon(weaponIndex)){
                playerInventory.TakeSouls(activeCustomer.weapons[weaponIndex].price);
            }else{
                Debug.Log("Failed to equip weapon!");
            }
        }else{
            Debug.Log("Not enough souls!");
        }
    }

    public void BuyFirstAbility(){
        int curRank = abilityManager.GetAbilityRank(AbilityManager.AbilityTypes.First);
        if(playerInventory.souls >= activeCustomer.firstSkill[curRank+1].price){
                abilityManager.IncreaseAbilityRank(AbilityManager.AbilityTypes.First);
                Debug.Log("FirstAbility increased to "+(int)abilityManager.GetAbilityRank(AbilityManager.AbilityTypes.First));
        }else{
            Debug.Log("Not enough souls!");
        }
        ShowSkills(activeCustomer);
    }

    public void BuySecondAbility(){
        int curRank = abilityManager.GetAbilityRank(AbilityManager.AbilityTypes.Second);
        if(playerInventory.souls >= activeCustomer.secondSkill[curRank+1].price){
                abilityManager.IncreaseAbilityRank(AbilityManager.AbilityTypes.Second);
                Debug.Log("SecondAbility increased to "+(int)abilityManager.GetAbilityRank(AbilityManager.AbilityTypes.Second));
        }else{
            Debug.Log("Not enough souls!");
        }
        ShowSkills(activeCustomer);
    }
}
