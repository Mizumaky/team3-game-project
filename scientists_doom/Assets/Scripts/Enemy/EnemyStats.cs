using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public float enemyHealth = 50;

	void Update () {

        if (enemyHealth <= 0) {
            KillEnemy();
        }

	}

    public void KillEnemy() {
        Destroy(gameObject);
    }
}
