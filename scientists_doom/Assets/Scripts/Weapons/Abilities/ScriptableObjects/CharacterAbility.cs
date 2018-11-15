using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "Ability", menuName = "Character Abilities/Ability", order = 1)]
public class CharacterAbility : ScriptableObject {
  public enum TravelDestination { self, ground, air }

  [Header ("Ability Settings")]
  [SerializeField] private string objectName = "Character Ability";
  [SerializeField] private Sprite icon;
  [SerializeField] private string animationTrigger = "attackTrigger";

  [Header ("Charging")]
  [SerializeField][Range (0f, 5f)] private float maxChargeTime = 0f;
  [SerializeField][Range (0.5f, 5f)] private float chargeSpeed = 0f;
  [SerializeField][Range (0f, 0.5f)] private float chargeStartDelay = 0f;
  [SerializeField] private bool dropDuringDelay = false;
  [SerializeField] private float startChargeFactor = 0f;

  [Header ("Travel")]
  [SerializeField] private bool isStationary;
  [SerializeField] private float velocity;
  [SerializeField] private TravelDestination destination = TravelDestination.ground;
  [SerializeField] private float travelHeight = 0.5f;
  [SerializeField] private float destinationHeight;
  [SerializeField] private float lifeTime = 1f;

  [Header ("Hit")]
  [SerializeField] private float baseHitRadius = 1f;
  [SerializeField] private float damage = 10f;
  [SerializeField] private float stunDuration = 0f;

  [Header ("Carry")]
  [SerializeField] private Transform weaponTransform;
  [SerializeField] private Transform casterTransform;

  [Header ("Prefab")]
  [SerializeField] private bool isInstanced;
  [SerializeField] private GameObject abilityPrefab;
  [SerializeField] private static GameObject chargingAbilityObject;
  [SerializeField] private CharacterAbilityInstance chargingAbilityScript;

  [Header ("Character Enhancement")]
  [SerializeField] private bool enhancesCharacter;
  [SerializeField] private Stats characterStats;

  #region GettersSetters
  public float GetStunDuration () {
    return stunDuration;
  }
  public string GetName () {
    return name;
  }

  public Sprite GetIcon () {
    return icon;
  }

  public string GetAnimationTrigger () {
    return animationTrigger;
  }

  public float GetMaxChargeTime () {
    return maxChargeTime;
  }

  public float GetChargeSpeed () {
    return chargeSpeed;
  }

  public float GetChargeStartDelay () {
    return chargeStartDelay;
  }

  public bool DropsDuringStartDelay () {
    return dropDuringDelay;
  }

  public float GetDamage () {
    return damage;
  }

  public Transform GetWeaponTransform () {
    return weaponTransform;
  }

  public Transform GetCasterTransform () {
    return casterTransform;
  }

  public bool hasInstance () {
    return isInstanced;
  }

  public GameObject GetAbilityPrefab () {
    return abilityPrefab;
  }

  public float GetStartChargeFactor () {
    return startChargeFactor;
  }

  public float GetLifeTime () {
    return lifeTime;
  }

  public float GetVelocity () {
    return velocity;
  }

  public TravelDestination GetTravelDestination () {
    return destination;
  }

  public float GetDestinationHeight () {
    return destinationHeight;
  }

  public float GetBaseHitRadius () {
    return baseHitRadius;
  }
  #endregion

  public virtual void Cast (Transform casterTransform, Transform weaponTransform) {
    this.weaponTransform = weaponTransform;
    this.casterTransform = casterTransform;
  }
}