using UnityEngine;
using UnityEngine.Networking;

public class PlayerAttacks_NET : NetworkBehaviour {

    public float distance = 50f;
    public float projectileVelocity = 20f;

    public GameObject projectilePrefab;
    public Transform spawnPosition;

    void Update() {

        if (!isLocalPlayer) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            CmdFire();
        }
	}

    [Command]
    void CmdFire() {

        //Vector3 target = (GetGroundPosition() - transform.position).normalized;

        //Spawn projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, spawnPosition.rotation) as GameObject;

        //Set velocity to projectile
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.up * projectileVelocity;

        NetworkServer.Spawn(projectile);

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
