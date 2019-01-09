using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRotator : MonoBehaviour
{
    public GameObject[] rotatingGems;
    public float rotatingSpeed;

    private GameObject[] spawnedGems;
    private Vector3[] rotationAxis;
    
    void Start()
    {
        spawnedGems = new GameObject[rotatingGems.Length];
        rotationAxis = new Vector3[rotatingGems.Length];
        for(int i = 0; i < rotatingGems.Length; i++){
            spawnedGems[i] = Instantiate(rotatingGems[i]);

            spawnedGems[i].transform.parent = transform;
            spawnedGems[i].transform.localPosition = Random.onUnitSphere / 2.3f;
            spawnedGems[i].transform.localEulerAngles = new Vector3(0,0,0);

            rotationAxis[i] = Vector3.Reflect(Random.onUnitSphere, spawnedGems[i].transform.localPosition);
        }
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 1);
        for(int i = 0; i < spawnedGems.Length; i++){
            spawnedGems[i].transform.RotateAround(transform.position, rotationAxis[i], Time.deltaTime * rotatingSpeed * Mathf.Pow(-1, i));
        }
    }
}
