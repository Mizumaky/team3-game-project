using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour{
    public static string EVENT_PLAYER_READY = "playerReady";
    public static string EVENT_LEVEL_ENDED = "levelEnded";

    public StoryLevel[] storyLevels;
    
    private EnemySpawner enemySpawner;
    private int storyLevelCount;
    private int curLevel;
    private Animator lightAnimator;
    private bool playerReady, levelPending;

    private void Start(){
        playerReady = false;
        lightAnimator = FindObjectOfType<SceneLighting>().gameObject.GetComponent<Animator>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        storyLevelCount = storyLevels.Length;
        curLevel = 1;
        StartLevel();
        EventManager.StartListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
        EventManager.StartListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && levelPending){
            playerReady = true;
        }
    }

    private void StartLevel(){
        StartCoroutine(StartLevelCor());
    }

    private IEnumerator StartLevelCor()
    {
        yield return new WaitForSeconds(3);
        //Prompt user to start next level (press *key* to start next level)
        Announcer.Announce("", "Press 'R' to start next level...");
        levelPending = true;
        while(!playerReady){
            yield return new WaitForSeconds(0.5f);
        }
        playerReady = levelPending = false;
        lightAnimator.SetTrigger("TriggerNight");
        yield return new WaitForSeconds(5);
        // User has signaled that the next level can start
        // Play evening transition
        // Wait a couple seconds after the animation's end
        // Start spawning and chnaging the lighting progress (because of moon shadows)
        if(curLevel > storyLevelCount){
            Debug.LogWarning("Level "+(curLevel)+" does not exist! Canceling StoryMode");
            EventManager.StopListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
            EventManager.StopListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
            yield return null;
        }
        Debug.Log("Starting StoryLevel "+storyLevels[curLevel]);
        Announcer.Announce(("Level "+(curLevel)+" begins"), "Defend the castle!");
        enemySpawner.StartSpawnWaveIfInactive(storyLevels[curLevel-1].peasantCount);
        curLevel++;
    }

    private void EndLevel(){
        StartCoroutine(EndLevelCor());
    }

    private IEnumerator EndLevelCor(){
        yield return new WaitForSeconds(10);
        lightAnimator.SetTrigger("TriggerDay");
        Announcer.Announce(("Level "+(curLevel)+" ended"), "You can rest for while");
        StartLevel();
        yield return null;
    }

    void OnDestroy()
    {
        EventManager.StopListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
        EventManager.StopListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
    }
}