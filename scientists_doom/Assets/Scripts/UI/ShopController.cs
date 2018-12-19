using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	public Customer[] customers;
    private Customer activeCustomer;
    public Image[] weaponImages;
    public Text[] weaponPrices;
    public GameObject[] firstSkillSlots = new GameObject[3];
    public GameObject[] secondSkillSlots = new GameObject[3];

    private void Start(){
        
    }
    void OnEnable(){
        activeCustomer = customers[(int)CharacterManager.activeCharacter];
        if(activeCustomer){
            Debug.Log("Shop Opened, Customer: "+activeCustomer);
            ShowWeapons(activeCustomer);
        }
    }

    private void ShowWeapons(Customer customer){
        for(int i = 0; i < 3; i++){
            weaponImages[i].sprite = customer.weapons[i].icon;
            weaponPrices[i].text = customer.weapons[i].price.ToString();
        }
    }
    private void ShowSkills(Customer customer){

    }
}
