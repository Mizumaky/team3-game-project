using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public AudioSource dayAudioSource, nightAudioSource;
  public float themeBlendSpeed;
  private AudioSource activeSource;

  private void Awake()
  {
    dayAudioSource.volume = 1;
    nightAudioSource.volume = 0;
    activeSource = dayAudioSource;
    EventManager.StartListening("dayTheme", BlendDay);
    EventManager.StartListening("nightTheme", BlendNight);
  }

  private void BlendDay()
  {
    StartCoroutine(BlendInDayTheme());
  }

  private void BlendNight()
  {
    StartCoroutine(BlendInNightTheme());
  }

  private IEnumerator BlendInDayTheme()
  {
    while (dayAudioSource.volume < 1)
    {
      dayAudioSource.volume += Time.deltaTime * themeBlendSpeed;
      nightAudioSource.volume = 1 - dayAudioSource.volume;
      yield return null;
    }
    activeSource = dayAudioSource;
  }

  private IEnumerator BlendInNightTheme()
  {
    Debug.Log("NIGHTTTTTT");
    while (nightAudioSource.volume < 1)
    {
      nightAudioSource.volume += Time.deltaTime * themeBlendSpeed;
      dayAudioSource.volume = 1 - nightAudioSource.volume;
      yield return null;
    }
  }
}
