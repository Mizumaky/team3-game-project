using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	public Customer[] customers;
    private Customer activeCustomer;
    public Button[] weaponButtons;
    public GameObject[] firstSkillSlots = new GameObject[3];
    public GameObject[] secondSkillSlots = new GameObject[3];
    private WeaponManager weaponManager;
    private PlayerStats playerStats;
    private Inventory playerInventory;

    void OnEnable(){
        
        activeCustomer = customers[(int)CharacterManager.activeCharacter];
        if(activeCustomer){
            weaponManager = FindObjectOfType<WeaponManager>();
            playerStats = CharacterManager.activeCharacterObject.GetComponent<PlayerStats>();
            playerInventory = CharacterManager.activeCharacterObject.GetComponent<Inventory>();
            Debug.Log("Shop Opened, Customer: "+activeCustomer+", Level: " + playerStats.GetCurrentHeroLevel());
            ShowWeapons(activeCustomer);
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
}
