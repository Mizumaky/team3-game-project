using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManagerOld : MonoBehaviour {
    //is attached to an object in scene (not prefab), so that i can assign transforms of turret spawnpoints that are in scene
    //probably castle, cuz it spawns a turret as a child of the object it is attached to

    //TODO change spawnpoint dle turretu, udelat nejaky dic nebo tak na zacatku

    public enum Turret { BarbarianTurret, WizardTurret, RangerTurret, WitchTurret }
    [Header("Turret Prefabs")]
    public GameObject barbarianTurretPrefab; //TODOOOO make it like character manager - only active turret, and spawn based on player
    public Transform barbarianTurretSpawnPoint;
    public GameObject wizardTurretPrefab;
    public Transform wizardTurretSpawnPoint;
    public GameObject rangerTurretPrefab;
    public Transform rangerTurretSpawnPoint;

    [Space]
    [Header("In-game script references")]
    public static GameObject barbarianTurretActive = null;
    public static GameObject wizardTurretActive = null;
    public static GameObject rangerTurretActive = null;


    private void Start()
    {

    }

    private void Update()
    {
        if     (barbarianTurretActive == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnNewTurret(Turret.BarbarianTurret, barbarianTurretSpawnPoint);
        } else if (wizardTurretActive == null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnNewTurret(Turret.WizardTurret, wizardTurretSpawnPoint); 
        } else if (rangerTurretActive == null && Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnNewTurret(Turret.RangerTurret, rangerTurretSpawnPoint); 
        }
    }

    private void SpawnNewTurret(Turret turret, Transform spawnPoint)
    {
        switch (turret)
        {
            case Turret.BarbarianTurret:
                barbarianTurretActive = Instantiate(barbarianTurretPrefab, spawnPoint.position, spawnPoint.rotation, this.transform);
                break;
            case Turret.WizardTurret:
                wizardTurretActive = Instantiate(wizardTurretPrefab, spawnPoint.position, spawnPoint.rotation, this.transform);
                break;
            case Turret.RangerTurret:
                rangerTurretActive = Instantiate(rangerTurretPrefab, spawnPoint.position, spawnPoint.rotation, this.transform);
                break;
            default:
                break;
        }


    }

    /*
    public void ChangeActiveCharacter(int index)
    {
        Transform oldTransform;
        if (activeCharacter != null)
        {
            oldTransform = activeCharacter.transform;
            Destroy(activeCharacter);
        }
        else
        {
            oldTransform = spawnPoint;
        }

        Turret newCharacter;
        switch (index)
        {
            default: newCharacter = Turret.Barbarian; break;
            case 1: newCharacter = Turret.Wizard; break;
        }
        SpawnNewCharacter(newCharacter, oldTransform);
    }
    */
}
