using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

  public enum Character { Barbarian, Wizard }
  [Header("Character Prefabs")]
  public GameObject wizard;
  public GameObject barbarian;

  [Header("On Start")]
  public bool spawnOnStart = false;
  public Character characterOnStart;
  public Transform spawnPoint;

  [Space]
  public GameObject activeCharacter;
  public Transform castleTransform;
  public DoubleFocusCamera dfcamera;

  private void Start() {
    if(spawnOnStart) { 
      SpawnNewCharacter(characterOnStart, spawnPoint);
    } else {
      dfcamera.focus = spawnPoint;
    }
  }

  private void SpawnNewCharacter(Character character, Transform transform) {
    GameObject newCharacter;
    switch(character) {
      default: newCharacter = barbarian; break;
      case Character.Wizard: newCharacter = wizard; break;
    }
    activeCharacter = Instantiate(newCharacter, transform.position, transform.rotation, this.transform);
    dfcamera.focus = activeCharacter.transform;
  }

  /// <summary>
  /// Destroys the old character and instantiates a new one at the same coordinates
  /// </summary>
  /// <param name="character">new character index</param>
  public void ChangeActiveCharacter(int index) {
    Transform oldTransform;
    if(activeCharacter != null) {
      oldTransform = activeCharacter.transform;
      Destroy(activeCharacter);
    } else {
      oldTransform = spawnPoint;
    }
    
    Character newCharacter;
    switch(index) {
      default: newCharacter = Character.Barbarian; break;
      case 1: newCharacter = Character.Wizard; break;
    }
    SpawnNewCharacter(newCharacter, oldTransform);
  }

}
