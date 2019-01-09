using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {
    //is attached to an object in scene (not prefab), so that i can assign transforms of turret spawnpoints that are in scene
    //probably castle, cuz it spawns a turret as a child of the object it is attached to

    //TODO change spawnpoint dle turretu, udelat nejaky dic nebo tak na zacatku

    public enum Turret { BarbarianTurret, WizardTurret, RangerTurret, WitchTurret }

    [Header ("Turrets and spawnpoints")]
    public GameObject barbarianTurretPrefab;
    public GameObject wizardTurretPrefab;
    public GameObject huntressTurrentPrefab;

    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;

    [Space]
    [Header ("In-game script references")]
    public static GameObject activeTurretObject = null;
    //public static Turret activeTurret;

    private void Start () {

    }

    private void Update () {
        if (activeTurretObject == null && Input.GetKeyDown (KeyCode.T)) {
            SpawnNewTurret (SpawnPoint1); //TODO multiplayer different spawnpoints
        }
    }

    private void SpawnNewTurret (Transform spawnPoint) {
        switch (CharacterManager.activeCharacter) {
            case CharacterManager.Character.Barbarian:
                activeTurretObject = Instantiate (barbarianTurretPrefab, spawnPoint.position, spawnPoint.rotation, this.transform);
                break;
            case CharacterManager.Character.Wizard:
                activeTurretObject = Instantiate (wizardTurretPrefab, spawnPoint.position, spawnPoint.rotation, this.transform);
                break;
            case CharacterManager.Character.Huntress:
                activeTurretObject = Instantiate (huntressTurrentPrefab, spawnPoint.position, spawnPoint.rotation, this.transform);
                break;
            default:
                break;
        }
    }
}