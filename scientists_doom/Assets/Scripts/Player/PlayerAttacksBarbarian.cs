﻿using UnityEngine;

public class PlayerAttacksBarbarian : MonoBehaviour {

    private Animator animator;
    public Collider axeCollider;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        axeCollider = GetComponentInChildren<BoxCollider>();
        axeCollider.enabled = false;
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            Fire();
        }
	}

    void Fire() {
        
        animator.SetTrigger("attackTrigger");

    }

    public void AxeSwingStart() {
        Debug.Log("Axe enabled");
        axeCollider.enabled = true;
    }

    public void AxeSwingEnd()
    {
        Debug.Log("Axe Disabled");
        axeCollider.enabled = false;
    }
}
