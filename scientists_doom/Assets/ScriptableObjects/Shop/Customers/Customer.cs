using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Customer", menuName = "Shop/Customer", order = 0)]
public class Customer : ScriptableObject
{
  public Weapon[] weapons;
  public AbilityRankData[] firstSkill;
  public AbilityRankData[] secondSkill;
  
  /*
    TODO: Add turrets, when implemented
   */
}