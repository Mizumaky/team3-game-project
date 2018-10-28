using UnityEngine;

public class PlayerAttacks : MonoBehaviour {

    public float distance = 50f;
    public float projectileVelocity = 20f;

    public GameObject projectilePrefab;
    public Transform spawnPosition;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Fire();
        }
	}

    void Fire() {
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, spawnPosition.rotation) as GameObject;

        //Set velocity to projectile
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.up * projectileVelocity;
        projectile.GetComponent<CharacterAbility>().casterTransform = transform;
        projectile.GetComponent<CharacterAbility>().damage = GetComponent<PlayerStats>().GetAttackDamage();
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
