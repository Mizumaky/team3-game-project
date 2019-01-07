using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelManager : MonoBehaviour
{
  public static string EVENT_PLAYER_READY = "playerReady";
  public static string EVENT_LEVEL_ENDED = "levelEnded";
  public static string EVENT_PLAYER_DEAD = "playerDead";
  public static string EVENT_CASTLE_DESTROYED = "castleDestroyed";

  public static string PREF_MAX_FINISHED_LEVEL = "highestLevel";

  public StoryLevel[] storyLevels;

  private EnemySpawner enemySpawner;
  private int storyLevelCount;
  private int curLevel, highestLevel;
  private Animator lightAnimator;
  private bool playerReady, levelPending;
  private PostProcessVolume pfx;
  private ColorGrading color;
  private bool levelActive;
  private Stats castleStats;
  private bool isGameOver;

  private void Start()
  {
    castleStats = GameObject.Find("Castle").GetComponent<Stats>();
    playerReady = false;
    lightAnimator = FindObjectOfType<SceneLighting>().gameObject.GetComponent<Animator>();
    enemySpawner = FindObjectOfType<EnemySpawner>();
    storyLevelCount = storyLevels.Length;
    highestLevel = PlayerPrefs.GetInt(PREF_MAX_FINISHED_LEVEL, 0);
    if (highestLevel < 0)
    {
      highestLevel = 0;
    }
    Debug.Log("Player finished level " + highestLevel + "before.");
    StartLevel(highestLevel + 1);

    EventManager.StartListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
    EventManager.StartListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
    EventManager.StartListening(LevelManager.EVENT_PLAYER_DEAD, EndGamePlayerDead);
    EventManager.StartListening(LevelManager.EVENT_CASTLE_DESTROYED, EndGameCastleDestroyed);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.R) && levelPending)
    {
      playerReady = true;
    }
    if (color != null)
    {
      if (color.postExposure.value > -100)
      {
        color.postExposure.Override(color.postExposure.value - 2 * Time.deltaTime);
      }
    }
  }

  public void SetPlayerReady()
  {
    if (levelPending)
    {
      playerReady = true;
    }
  }

  private void StartLevel()
  {
    if (!levelActive)
    {
      StartCoroutine(StartLevelCor(highestLevel));
    }
    else
    {
      Debug.LogWarning("Level already in progress!");
    }
  }

  public void StartLevel(int levelNumber)
  {
    if (!levelActive)
    {
      StartCoroutine(StartLevelCor(levelNumber));
    }
    else
    {
      Debug.LogWarning("Level already in progress!");
    }
  }

  private IEnumerator StartLevelCor(int levelNumber)
  {
    if (levelNumber > storyLevelCount)
    {
      Debug.LogWarning("Level " + (levelNumber) + " does not exist! Canceling StoryMode");
      OnDestroy();
      yield return null;
    }
    castleStats.ResetHealth();
    EventManager.TriggerEvent("updateHUD");
    Debug.Log("Starting level " + levelNumber);
    curLevel = levelNumber;

    yield return new WaitForSeconds(3);
    //Prompt user to start next level (press *key* to start next level)
    Announcer.Announce("", "Press 'R' to start next level...");
    EventManager.TriggerEvent("levelReady");
    levelPending = true;
    while (!playerReady)
    {
      yield return new WaitForSeconds(0.5f);
    }
    playerReady = levelPending = false;
    levelActive = true;
    EventManager.TriggerEvent("levelInProgess");
    lightAnimator.SetTrigger("TriggerNight");
    EventManager.TriggerEvent("nightTheme");
    yield return new WaitForSeconds(5);

    Debug.Log("Starting StoryLevel " + storyLevels[levelNumber - 1]);
    Announcer.Announce(("Level " + (levelNumber) + " begins"), "Defend the castle!");
    enemySpawner.StartSpawnWaveIfInactive(storyLevels[levelNumber - 1]);
  }

  private void EndLevel()
  {
    if (levelActive)
    {
      StartCoroutine(EndLevelCor());
    }
    else
    {
      Debug.LogWarning("No level currently active, cant end it!");
    }
  }

  private IEnumerator EndLevelCor()
  {
    yield return new WaitForSeconds(2);
    lightAnimator.SetTrigger("TriggerDay");
    EventManager.TriggerEvent("dayTheme");
    Announcer.Announce(("Level " + (curLevel) + " ended"), "You can rest for while");
    levelActive = false;
    StartLevel(curLevel + 1);
    yield return null;
  }

  private void EndGameCastleDestroyed()
  {
    if (!isGameOver)
    {
      Announcer.Announce("You Lose!", "The Castle Has Fallen");
      StartCoroutine(GameOver());
    }
  }

  private void EndGamePlayerDead()
  {
    if (!isGameOver)
    {
      Announcer.Announce("You lose!", "The Character Has Died");
      StartCoroutine(GameOver());
    }
  }

  private IEnumerator GameOver()
  {
    color = ScriptableObject.CreateInstance<ColorGrading>();
    color.enabled.Override(true);
    color.saturation.Override(-100f);
    pfx = PostProcessManager.instance.QuickVolume(8, 100, color);

    yield return new WaitForSeconds(5f);

    RuntimeUtilities.DestroyVolume(pfx, true);
    FindObjectOfType<UIController>().ToggleWindow(FindObjectOfType<UIController>().retryWindow);
    yield return null;
  }

  void OnDestroy()
  {
    if (curLevel == 0)
    {
      PlayerPrefs.SetInt(PREF_MAX_FINISHED_LEVEL, 0);
    }
    PlayerPrefs.SetInt(PREF_MAX_FINISHED_LEVEL, curLevel - 1);
    EventManager.StopListening(LevelManager.EVENT_PLAYER_READY, StartLevel);
    EventManager.StopListening(LevelManager.EVENT_LEVEL_ENDED, EndLevel);
    EventManager.StopListening(LevelManager.EVENT_PLAYER_DEAD, EndGamePlayerDead);
    EventManager.StopListening(LevelManager.EVENT_CASTLE_DESTROYED, EndGameCastleDestroyed);
    StopAllCoroutines();
  }
}