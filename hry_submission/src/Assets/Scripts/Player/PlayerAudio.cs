using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class PlayerAudio : MonoBehaviour {
    [Header ("Audio Clips")]
    public AudioClip footStep;
    [Range (0f, 1f)]
    public float footStepVolume = 0.35f;
    [Space]
    public AudioSource audioSource;
    private Animator animator;

    private void Awake () {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        audioSource.clip = footStep;
    }
    private void Start () {
        audioSource.volume = footStepVolume;
        audioSource.spatialBlend = 1;
    }

    public void Step () {
        if(animator.GetFloat("speedParam") > 0.01f){
            audioSource.pitch = Random.Range (0.9f, 1.1f);
            audioSource.Play ();
        }
    }
}