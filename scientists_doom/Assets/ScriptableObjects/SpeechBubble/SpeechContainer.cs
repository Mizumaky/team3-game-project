using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speech", menuName = "Speech/SpeechContainer", order = 0)]
public class SpeechContainer : ScriptableObject
{
    public enum Mood { Happy, Angry, Motivational }
    
    public string[] messages;
    public Mood mood;
}
