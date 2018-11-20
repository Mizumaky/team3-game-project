using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class PlayerAudio : MonoBehaviour {
    [Header ("Audio Clips")]
    public AudioClip footStep;
    [Range (0f, 1f)]
    public float footStepVolume = 0.35f;
    [Space]
    public AudioSource audioSource;

    private void Awake () {
        audioSource = GetComponent<AudioSource> ();
        audioSource.clip = footStep;
    }
    private void Start () {
        audioSource.volume = footStepVolume;
        audioSource.spatialBlend = 1;
    }

    public void Step () {
        audioSource.pitch = Random.Range (0.9f, 1.1f);
        audioSource.Play ();
    }
}