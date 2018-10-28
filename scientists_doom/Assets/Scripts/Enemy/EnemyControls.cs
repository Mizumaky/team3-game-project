﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyControls : MonoBehaviour {

    private NavMeshAgent navMeshAgent;
    public Transform castle;
    private Transform target;
    private NavMeshPath path;
    [Range(5f, 10f)]
    public float distanceToFollowPlayer = 7f;
    private Animator animator;
    private float speed;
    private Vector3 lastPosition;

    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        target = castle;
        SetPathToTarget(target);
        animator = GetComponent<Animator>();
        speed = 0;
    }

    private void Update()
    {
        if (GetComponent<EnemyStats>().enemyAlive) {
            if (animator != null) {
                speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude / Time.deltaTime, 0.5f) / navMeshAgent.speed;
                lastPosition = transform.position;

                animator.SetFloat("speedParam", speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other) //colision with projectile
    {
        if (other != null && other.gameObject.layer == 11) // 11. layer hit enemies
        {
            float dmg;
            if (other.gameObject.GetComponentInParent<PlayerAttacksBarbarian>()) {
                target = other.GetComponentInParent<PlayerAttacksBarbarian>().transform;
                dmg = other.GetComponentInParent<PlayerStats>().GetAttackDamage();
            }
            else
            {
                target = other.GetComponentInChildren<CharacterProjectile>().casterTransform;
                dmg = other.GetComponentInChildren<CharacterProjectile>().damage;
            }
            GetComponent<EnemyStats>().TakeDamage(dmg);
        }
        StartCoroutine(CheckForTarget());
    }

    IEnumerator CheckForTarget()
    {
        if (GetComponent<EnemyStats>().enemyAlive)
        {
            while (target != null && target.gameObject.activeSelf && Vector3.Distance(target.position, transform.position) < distanceToFollowPlayer)
            {
                SetPathToTarget(target);
                yield return new WaitForSeconds(0.4f);
            }
            target = castle;
            SetPathToTarget(target);
        }
    }

    private void SetPathToTarget(Transform target)
    {
        if (target != null && gameObject != null)
        {
            if (navMeshAgent.enabled && NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
            {
                navMeshAgent.SetPath(path);
            }
            else
            {
                Debug.Log("Enemy could not set path");
            }
        }
    }

    public void DisableMovement()
    {
        StopAllCoroutines();
        navMeshAgent.ResetPath();
    }

    public void DisableCollision()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }


}
