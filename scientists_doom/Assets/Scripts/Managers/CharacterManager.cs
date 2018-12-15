using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

  public enum Character { Barbarian, Wizard, Huntress, noCharacter }

  [Header("Character Prefabs")]
  public GameObject wizard;
  public GameObject barbarian;
  public GameObject huntress;

  [Header("On Start")]
  public bool spawnOnStart = false;
  public Character characterOnStart;
  public Transform spawnPoint;
  public GameObject cameraParentReference;

  [Space]
  public static GameObject activeCharacterObject = null; //BTW statics OK even when multi - static variables are not shared between clients
  public static Character activeCharacter = Character.noCharacter;

  private DoubleFocusCamera dfcameraReference;
  private TurretManager turretManagerScript;

  private void Start()
  {
    dfcameraReference = cameraParentReference.GetComponentInChildren<DoubleFocusCamera>();
    turretManagerScript = GetComponent<TurretManager>();

    if (spawnOnStart)
    {
      SpawnNewCharacter(characterOnStart, spawnPoint);
    }
    else
    {
      dfcameraReference.focus = spawnPoint;
    }
  }

  private void SpawnNewCharacter(Character character, Transform transform)
  {
    GameObject newCharacter;
    switch (character)
    {
      case Character.Wizard:
        newCharacter = wizard;
        activeCharacter = Character.Wizard;
        break;
      case Character.Huntress:
        newCharacter = huntress;
        activeCharacter = Character.Huntress;
        break;
      default:
        newCharacter = barbarian;
        activeCharacter = Character.Barbarian;
        break;
    }
    activeCharacterObject = Instantiate(newCharacter, transform.position, transform.rotation, this.transform);
    dfcameraReference.focus = activeCharacterObject.transform;
    EventManager.TriggerEvent("changeCharSpec");
  }

  /// <summary>
  /// Destroys the old character and instantiates a new one at the same coordinates
  /// </summary>
  /// <param name="character">new character index</param>
  public void ChangeActiveCharacter(int index)
  {
    Transform oldTransform;
    if (activeCharacterObject != null)
    {
      oldTransform = activeCharacterObject.transform;
      Destroy(activeCharacterObject);
    }
    else
    {
      oldTransform = spawnPoint;
    }

    Character newCharacter;
    switch (index)
    {
      case 1:
        newCharacter = Character.Wizard;
        break;
      case 2:
        newCharacter = Character.Huntress;
        break;
      default:
        newCharacter = Character.Barbarian;
        break;
    }
    SpawnNewCharacter(newCharacter, oldTransform);
  }

}