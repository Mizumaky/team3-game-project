using UnityEngine;

public class WeaponManager : MonoBehaviour
{
  public GameObject[] weapons;
  public WeaponData[] weaponData;
  public int activeWeaponIndex;
  private GameObject activeWeapon;
  private Transform hand;
  public Vector3 attachPosition;
  private PlayerStats stats;

  void Awake()
  {
    hand = transform.FindDeepChild("Hand_L");
    if (hand)
    {
      activeWeaponIndex = 0;
      EquipWeapon(activeWeaponIndex);
    }
    else
    {
      Debug.Log("Could not find the characters hand!!!");
    }
  }
  ///<summary>
  ///Equip weapon of tier weaponIndex(0 - starting, 1 - advanced, 2 - master)
  ///</summary>
  public bool EquipWeapon(int weaponIndex)
  {
    if (hand.childCount > 0)
    {
      GetComponent<PlayerStats>().RemoveBonusDamage(weaponData[activeWeaponIndex].damage);
      Destroy(hand.GetChild(0).gameObject);
    }
    activeWeaponIndex = weaponIndex;
    GetComponent<PlayerStats>().AddBonusDamage(weaponData[activeWeaponIndex].damage);

    Debug.Log("Spawning weapon(" + weapons[weaponIndex].name + ")");
    activeWeapon = Instantiate(weapons[weaponIndex]);
    activeWeapon.transform.parent = hand;
    activeWeapon.transform.localPosition = attachPosition;
    activeWeapon.transform.localEulerAngles = new Vector3(0, 0, 0);

    return true;
  }
}