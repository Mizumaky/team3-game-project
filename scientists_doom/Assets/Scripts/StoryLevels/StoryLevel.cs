using UnityEngine;

[CreateAssetMenu (fileName = "StoryLevel", menuName = "StoryLevel", order = 2)]
public class StoryLevel : ScriptableObject {
	[Range(0, 10)]
	private int levelNo;
	private int enemyCount;	
}