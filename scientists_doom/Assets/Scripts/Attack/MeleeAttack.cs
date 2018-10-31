using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    private BoxCollider weponCollider;
    public float weponDamage;
    private float aditionalDamage; //character damage
    private float totalDamage; //sum of wepon and aditional dmg
    private int layerToHit;

    void Awake() {
        weponCollider = GetComponent<BoxCollider>();
        weponCollider.enabled = false;

        //decides if will hit enemy or player
        if (gameObject.layer == 10) {
            layerToHit = 11;
            aditionalDamage = gameObject.GetComponentInParent<EnemyStats>().enemyDamage;
        } else if (gameObject.layer == 11) {
            layerToHit = 10;
            aditionalDamage = gameObject.GetComponentInParent<PlayerStats>().GetAttackDamage();
        } else {
            Debug.Log("Object with this script has not Enemy or Player layer");
        }
        totalDamage = weponDamage + aditionalDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        int otherLayer = other.gameObject.layer;
        if (layerToHit == otherLayer && other.gameObject.tag != "Weapon") {
            TakeDamageToOther(other.gameObject);
        } 
    }

    //decides who is other and take damage
    private void TakeDamageToOther(GameObject other) {
        if(other.layer == 10) {
            other.GetComponent<EnemyStats>().TakeDamage(totalDamage);
        } else{
            other.GetComponent<PlayerStats>().TakeDamage(totalDamage);
        }
    }
}
