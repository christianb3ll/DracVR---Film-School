using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateReplay;
using UltimateReplay.Storage;

// Manages the replay of handheld camera movement
public class CameraReplays : MonoBehaviour
{
    private ReplayStorage storage;
    private ReplayRecordOperation recordOp;
    private ReplayPlaybackOperation playbackOp;

    private ReplayScene scene;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the storage references
        storage = new ReplayMemoryStorage("CameraStorage");
        // scene = new ReplayScene();
    }

    // Adds ann object to the replay scene
    public void AddToScene(GameObject obj)
    {
        scene.AddReplayObject(obj);
    }

    // Removes an object from the replay scene
    public void RemoveFromScene(GameObject obj)
    {
        scene.RemoveReplayObject(obj);
    }

    // Begins a new recording
    public void StartRecording()
    {
        recordOp = ReplayManager.BeginRecording(storage);
    }

    // Ends a recording
    public void EndRecording()
    {
        if (recordOp.IsRecording)
        {
            recordOp.StopRecording();
        }
        
    }

    // Begins playback of a recording
    public void StartPlayback()
    {
        playbackOp = ReplayManager.BeginPlayback(storage);
    }

    // Ends playback
    public void EndPlayback()
    {
        if (playbackOp.IsReplaying)
        {
            playbackOp.StopPlayback();
        }
    }
}
