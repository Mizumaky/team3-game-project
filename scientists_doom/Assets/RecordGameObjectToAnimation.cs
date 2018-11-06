using UnityEditor.Experimental.Animations;
using UnityEngine;

public class RecordGameObjectToAnimation : MonoBehaviour {

    public AnimationClip clip;

    private GameObjectRecorder gameObjectRecorder;

	void Start () {
        // Create recorder and record the script GameObject.
        gameObjectRecorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        gameObjectRecorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    void LateUpdate()
    {
        if (clip == null)
            return;

        // Take a snapshot and record all the bindings values for this frame.
        gameObjectRecorder.TakeSnapshot(Time.deltaTime);
    }

    void OnDisable()
    {
        if (clip == null)
            return;

        if (gameObjectRecorder.isRecording)
        {
            // Save the recorded session to the clip.
            gameObjectRecorder.SaveToClip(clip);
        }
    }
}
