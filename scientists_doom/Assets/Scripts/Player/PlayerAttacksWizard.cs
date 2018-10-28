using UnityEngine;

public class PlayerAttacksWizard : PlayerController {

    public float distance = 50f;
    public float projectileVelocity = 20f;

    public GameObject projectilePrefab;
    public Transform spawnPosition;

    void Update() {
        if (currentState == PlayerState.movingState && Input.GetKeyDown(KeyCode.Space)) {
            Fire();
        } 
	}

    void jumpToTurret ()
    {
        Debug.Log("jumping to turret");
    }
    void jumpFromTurret()
    {
        Debug.Log("jumping from turret");
    }

    void Fire() {
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, spawnPosition.rotation) as GameObject;

        //Set velocity to projectile
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.up * projectileVelocity;
        projectile.GetComponent<CharacterAbility>().casterTransform = transform;
    }

    Vector3 GetGroundPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            
            return hit.point;
        }
        return GetComponent<Transform>().position;
    }
}
