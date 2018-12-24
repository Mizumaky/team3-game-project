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

    private void Start(){
        lightAnimator = FindObjectOfType<SceneLighting>().gameObject.GetComponent<Animator>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        storyLevelCount = storyLevels.Length;
        curLevel = 0;
        StartLevel();
        EventManager.StartListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
        EventManager.StartListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
    }

    private void StartLevel(){
        StartCoroutine(StartLevelCor());
    }

    private IEnumerator StartLevelCor()
    {
        lightAnimator.SetTrigger("TriggerNight");
        yield return new WaitForSeconds(5);
        // User has signaled that the next level can start
        // Play evening transition
        // Wait a couple seconds after the animation's end
        // Start spawning and chnaging the lighting progress (because of moon shadows)
        if(curLevel >= storyLevelCount){
            Debug.LogWarning("Level "+(curLevel+1)+" does not exist! Canceling StoryMode");
            EventManager.StopListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
            EventManager.StopListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
            yield return null;
        }
        Debug.Log("Starting StoryLevel "+storyLevels[curLevel]);
        Announcer.Announce(("Level "+(curLevel+1)+" begins"), "Defend the castle!");
        enemySpawner.StartSpawnWaveIfInactive(storyLevels[curLevel].peasantCount);
        curLevel++;
    }

    private void EndLevel(){

    }

    private IEnumerator EndLevelCor(){
        Announcer.Announce(("Level "+(curLevel+1)+" ended"), "You can rest for while");
        yield return null;
    }

    void OnDestroy()
    {
        EventManager.StopListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
        EventManager.StopListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
    }
}