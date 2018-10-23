using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 20f;
    [Header("Hit Effect")]
    public GameObject impactEffect;
    public AudioClip soundClip;
    public AudioSource soundSource;
    [Header("Flying Animation")]
    public bool animated;
    public Animator animator;
    public string animationName; //name of the animation to play

    private Transform target;

    public void PassToBulletScript(Transform importedTarget) //func to be called from outside
    {
        target = importedTarget;
    }

    void Awake()
    {
        if (animated)
        {
            animator = GetComponent<Animator>();
            animator.Play(animationName);
        }
    }

    void Update () {
		if (target == null) //just in case target gets killed or so before it hits him
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position; //vector where to go
        float distanceToMoveThisFrame = speed * Time.deltaTime;
        if (distanceToMoveThisFrame >= dir.magnitude) //if we would get past the target position, then its already a hit
        {
            HitTarget();
            return;
        }
        //otherwise continue moving
        transform.Translate(dir.normalized * distanceToMoveThisFrame, Space.World);

	}

    void HitTarget()
    {
        //Debug.Log("Target hit!");
        Quaternion particleRot = Quaternion.LookRotation(target.position - transform.position);
        GameObject particle = Instantiate(impactEffect, transform.position, particleRot);
        soundSource.clip = soundClip;
        soundSource.Play();
        Destroy(particle, 2f);
        Destroy(gameObject);
    }
}
