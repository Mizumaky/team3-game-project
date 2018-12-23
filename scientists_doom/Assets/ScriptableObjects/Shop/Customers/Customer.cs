using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Customer", menuName = "Shop/Customer", order = 0)]
public class Customer : ScriptableObject
{
  public ShopWeapon[] weapons;
  public ShopAbility[] firstSkill;
  public ShopAbility[] secondSkill;
  
  /*
    TODO: Add turrets, when implemented
   */
}