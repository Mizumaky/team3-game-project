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

    void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        target = castle;
        SetPathToTarget(target);
    }
    
    private void OnTriggerEnter(Collider other) //colision with projectile
    {
        if (other.gameObject.layer == 11) // 11. layer hit enemies
        {
            GetComponent<EnemyStats>().enemyHealth -= 10;
            if (other.gameObject.GetComponentInParent<PlayerAttacksBarbarian>()) {
                target = other.GetComponentInParent<PlayerAttacksBarbarian>().transform;
            }
            else
            {
                target = other.GetComponentInChildren<CharacterProjectile>().casterTransform;
            }
        }
        StartCoroutine(CheckForTarget());
    }

    IEnumerator CheckForTarget()
    {
        while (target != null && target.gameObject.activeSelf && Vector3.Distance(target.position, transform.position) < distanceToFollowPlayer)
        {
            SetPathToTarget(target);
            yield return new WaitForSeconds(0.4f);
        }
        target = castle;
        SetPathToTarget(target);
    }

    private void SetPathToTarget(Transform target)
    {
        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            navMeshAgent.SetPath(path);
        }
        else
        {
            Debug.Log("Enemy could not set path");
        }
    }
}
