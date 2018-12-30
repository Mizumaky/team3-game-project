using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleInterface : MonoBehaviour
{
    public GameObject bubblePrefab;
    public SpeechContainer[] speeches;

    private bool speechActive;
    private Vector3 spawnPosition;
    private GameObject bubble;
    private SpeechBubbleController speechController;

    void Start()
    {
        spawnPosition = Vector3.zero;
        spawnPosition.y = GetComponent<Collider>().bounds.extents.y * 2;
        SaySomething(SpeechContainer.Mood.Motivational);
    }

    public void SpawnSpeechBubble(){
        bubble = Instantiate(bubblePrefab);
        bubble.transform.SetParent(gameObject.transform, false);
        bubble.transform.localPosition = spawnPosition;

        speechController = bubble.GetComponent<SpeechBubbleController>();
    }

    public void SaySomething(SpeechContainer.Mood mood){
        SpeechContainer speech = System.Array.Find(speeches, x => x.mood == mood);
        if(speech != null){
            if(bubble == null){
                SpawnSpeechBubble();
            }
            speechController.Show(speech.messages[Random.Range(0,speech.messages.Length)], 5f);
        }else{
            Debug.LogWarning(gameObject.name+" does not have a speech set for "+mood);
        }
    }
}
