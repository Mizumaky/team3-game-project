using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    private Transform cameraTransform; 
    public TextMeshProUGUI textMesh;

    void Start(){
        cameraTransform = GameObject.Find("Camera").transform;
    }

    void Update(){
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }

    private void ChangeText(string text){
        textMesh.text = text;
    }

    private void SetActive(bool state){
        transform.GetChild(0).gameObject.SetActive(state);
    }

    private void SetActiveFor(float time){
        StartCoroutine(SetActiveForCor(time));
    }

    private IEnumerator SetActiveForCor(float time){
        SetActive(true);
        yield return new WaitForSeconds(time);
        SetActive(false);
    }

    public void Show(string text, float time){
        ChangeText(text);
        if(time == -1){
            SetActive(true);
        }else{
            SetActiveFor(time);
        }
    }
}
