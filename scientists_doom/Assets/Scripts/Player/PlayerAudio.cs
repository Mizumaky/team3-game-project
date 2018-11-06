using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    [Header("Audio Clips")]
    public AudioClip footStep;
    [Range(0f, 1f)]
    public float footStepVolume;
    [Space]
    public AudioSource audioSource;

    void Start () {
        audioSource.volume = footStepVolume;
	}

    public void Step() {
        audioSource.clip = footStep;
        audioSource.Play();
    }
}
