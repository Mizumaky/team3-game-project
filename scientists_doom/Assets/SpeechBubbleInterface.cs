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
    }

    public void SpawnSpeechBubble(){
        bubble = Instantiate(bubblePrefab);
        bubble.transform.SetParent(gameObject.transform, false);
        bubble.transform.localPosition = spawnPosition;

        speechController = bubble.GetComponent<SpeechBubbleController>();
    }

    public void SaySomething(SpeechContainer.Mood mood){
        SaySomething(mood, 5f);
    }

    public void SaySomething(SpeechContainer.Mood mood, float time){
        SaySomething(mood, time, 1f);
    }

    public void SaySomething(SpeechContainer.Mood mood, float time, float probability){
        float rand = Random.Range(0f, 1f);
        
        if(rand <= probability){
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
}
