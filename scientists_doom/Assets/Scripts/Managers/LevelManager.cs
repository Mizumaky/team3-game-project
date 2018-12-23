using UnityEngine;

public class LevelManager : MonoBehaviour{
    public StoryLevel[] storyLevels;
    
    private EnemySpawner enemySpawner;
    private int storyLevelCount;
    private int curLevel;

    private void Start(){
        enemySpawner = FindObjectOfType<EnemySpawner>();
        storyLevelCount = storyLevels.Length;
        curLevel = 0;
        StartLevel();
        EventManager.StartListening("waveDone", StartLevel);
    }

    public void StartLevel(){
        if(curLevel >= storyLevelCount){
            Debug.LogWarning("Level "+(curLevel+1)+" does not exist! Canceling StoryMode");
            EventManager.StopListening("waveDone", StartLevel);
            return;
        }
        Debug.Log("Starting StoryLevel "+storyLevels[curLevel]);
        Announcer.Announce(("Level "+(curLevel+1)+" begins"), "Defend the castle!");
        enemySpawner.StartSpawnWaveIfInactive(storyLevels[curLevel].peasantCount);
        curLevel++;
    }

    void OnDestroy()
    {
        EventManager.StopListening("waveDone", StartLevel);
    }
}