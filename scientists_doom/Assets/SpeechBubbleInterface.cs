using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleInterface : MonoBehaviour
{
    public GameObject bubblePrefab;

    private Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = Vector3.zero;
        spawnPosition.y = GetComponent<Collider>().bounds.extents.y * 2;
        SpawnSpeechBubble("The castle shall fall!!!", 10f);
    }

    public void SpawnSpeechBubble(string text, float duration){
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.SetParent(gameObject.transform, false);
        bubble.transform.localPosition = spawnPosition;

        bubble.GetComponent<SpeechBubbleController>().Show(text, duration);
    }
}
