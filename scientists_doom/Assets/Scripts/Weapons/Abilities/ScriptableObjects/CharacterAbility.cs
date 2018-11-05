using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Ability", menuName = "Character Abilities/Ability", order = 1)]
public class CharacterAbility : ScriptableObject {

  [Header ("Ability Settings")]
  [SerializeField] protected string objectName = "Character Ability";
  [SerializeField] protected Sprite icon;

  [Header ("Charging")]
  [Range (0f, 5f)]
  [SerializeField] protected float maxChargeTime = 0f;
  [SerializeField] protected float currentChargeModifier;
  [Range (1f, 5f)]
  [SerializeField] protected float chargeSpeed = 0f;

  [Header ("Parameters")]
  [SerializeField] protected float damage = 10f;

  protected Transform weaponTransform;

  protected Transform casterTransform;
  [Header ("Prefab")]
  [SerializeField] private bool instance;
  [SerializeField] private GameObject abilityPrefab;
  [SerializeField] private static GameObject chargingAbilityObject;
  [SerializeField] private CharacterAbilityInstance chargingAbilityScript;

  #region GettersSetters
  public string GetName () {
    return name;
  }

  public Sprite GetIcon () {
    return icon;
  }

  public float GetMaxChargeTime () {
    return maxChargeTime;
  }

  public float GetChargeSpeed () {
    return chargeSpeed;
  }

  public float GetDamage () {
    return damage;
  }

  public Transform GetWeaponTransform () {
    return weaponTransform;
  }

  public bool hasInstance () {
    return instance;
  }

  public GameObject GetAbilityPrefab () {
    return abilityPrefab;
  }
  #endregion

  public virtual void Cast (Transform casterTransform, Transform weaponTransform) {
    this.weaponTransform = weaponTransform;
    Debug.Log ("Casting " + objectName + "!");
  }

  public virtual void UpdateCharge (float currentChargeModifier) {
    this.currentChargeModifier = currentChargeModifier;
    Debug.Log ("Updating " + objectName + "'s charge!");
  }

  public virtual void Release (float statPlusWeaponDamage) {
    Debug.Log ("Releasing " + objectName + " with total damage " + damage + " ability damage + " + statPlusWeaponDamage + " stat plus weapon damage!");
  }

}