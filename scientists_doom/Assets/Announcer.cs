using System.Collections;
using TMPro;
using UnityEngine;

public class Announcer : MonoBehaviour
{
    public static Announcer Instance { get; private set;}

    public GameObject container;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI secondaryText;

    void Awake(){
        if (Instance != null){
            return;
        }
        Instance = this;
    }

    void Start(){
        container.SetActive(false);    
    }

    public static void Announce(string mainMsg, string secondaryMsg){
        Announcer.Instance.AnnounceIntern(mainMsg, secondaryMsg);
    }

    private void AnnounceIntern(string mainMsg, string secondaryMsg){
        mainText.text = mainMsg;
        secondaryText.text = secondaryMsg;

        StartCoroutine(ShowMsg(2));
    }

    private IEnumerator ShowMsg(int sec){
        container.SetActive(true);
        yield return new WaitForSeconds(sec);
        container.SetActive(false);
    }
}
