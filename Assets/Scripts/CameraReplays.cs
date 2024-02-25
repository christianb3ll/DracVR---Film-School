using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateReplay;
using UltimateReplay.Storage;

// Manages the replay of handheld camera movement
public class CameraReplays : MonoBehaviour
{
    private UltimateReplay.ReplayManager manager;
    private ReplayStorage storage;
    private ReplayRecordOperation recordOp;
    private ReplayPlaybackOperation playbackOp;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the manager and storage references
        manager = GetComponent<ReplayManager>();
        storage = new ReplayMemoryStorage("CameraReplay");
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