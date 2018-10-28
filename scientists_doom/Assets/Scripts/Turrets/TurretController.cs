using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour {

    [Header("Aiming")]
    public float range = 20f;
    public float FOV = 170f;
    public float turnSpeed = 10f;
    public float searchInterval = 0.5f;
    [Header("Firing")]
    public float fireRate = 3f;
    public AudioClip soundClip;
    [Header("Firing Animation")]
    public bool animated = false;
    public string animationName; //animation asset name
    public float animationOffset = 0.08f; //time how much earlier should the animation start
    [Header("Other settings")]
    public GameObject bulletPrefab;
    public string enemyTag = "enemy";

    [Space]
    [Header("In-game script references")]
    public bool searchingActive = true;

    private Animator animator;
    private Transform partsToRotate;
    private Transform firePoint;
    private Transform targetTransform; //the turret will aim here
    private AudioSource soundSource;
    private float firingCountdown = 1f; //actual time remaining before next fire

    void Start () {
        if (animated)
        {
            animator = GetComponentInChildren<Animator>();
        }
        soundSource = GetComponent<AudioSource>();
        soundSource.clip = soundClip;
        partsToRotate = transform.GetChild(0).Find("RotationPoint");
        firePoint = partsToRotate.Find("FirePoint");
        if (partsToRotate == null || firePoint == null)
        {
            Debug.Log("error when getting parts, make sure thisprefab/anyturret/RotationPoint/FirePoint exists in the prefab");
        } else
        {
            StartCoroutine(RepeatSearchTarget(searchInterval)); //start intervalled search routine
        }
	}

    IEnumerator RepeatSearchTarget(float searchInterval)
    {
        while (searchingActive)
        {
            SearchTarget();
            yield return new WaitForSeconds(searchInterval);
        }
        //if searching not active then coroutine ends and need to be started again
    }

    void SearchTarget() //search closest object tagged enemy && in range && in fov
    {
        //Debug.Log("Searching...");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //get all tagged
        GameObject closestEnemy = null; //reset closest
        float closestEnemyDistance = Mathf.Infinity; //reset closest distance

        foreach (GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (enemyDistance <= range) //CHECK DISTANCE
            {
                Vector3 enemyDir = enemy.transform.position - transform.position; //get direction to enemy
                float enemyAngle = Vector3.Angle(transform.forward, enemyDir); //get angle to enemy
                if (enemyAngle <= FOV * 0.5f) //CHECK ANGLE
                {
                    if (enemy.GetComponent<EnemyStats>().enemyAlive) //CHECK IF ALIVE
                    {
                        if (enemyDistance < closestEnemyDistance) //CHECK IF CLOSEST
                        {
                            closestEnemyDistance = enemyDistance;
                            closestEnemy = enemy;
                        }
                    }
                }
            }

        }     
        if (closestEnemy != null) //if target found
        {
            //Debug.Log("Found a new target!");
            targetTransform = closestEnemy.transform;
        } else {
            targetTransform = null;
        }
    }


    void Update()
    {
        if (targetTransform == null) { return; }; //do nothing if no target

        rotateTowardsTarget();

        if (animated && firingCountdown <= animationOffset) //start animation by animationOffset earlier
        {
            animator.Play(animationName);
        } 

        if (firingCountdown <= 0)
        {
            Fire();
            firingCountdown = 1 / fireRate;
        }
        firingCountdown -= Time.deltaTime;
    }

    void rotateTowardsTarget()
    {
        Vector3 dir = targetTransform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir); //get rotation to that target
        Quaternion lookRotSmooth = Quaternion.Lerp(partsToRotate.transform.rotation, lookRot, Time.deltaTime * turnSpeed); //interpolate for smoothness
        Vector3 lookRotEuler = lookRotSmooth.eulerAngles; //convert to euler so only y axis cen be extracted
        partsToRotate.transform.rotation = Quaternion.Euler(0f, lookRotEuler.y, 0f);
    }

    void Fire()
    {
        //Debug.Log("booom!");
        soundSource.Play(); //play shooting sound
        GameObject FiredBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletController bulletScript = FiredBullet.GetComponent<BulletController>(); //get bullet script for passing target to it through its function
        if (bulletScript != null)
        {
            bulletScript.PassToBulletScript(targetTransform);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
