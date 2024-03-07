using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateReplay;
using UltimateReplay.Storage;

// Class for storing camera replay operations and methods
public class CameraReplay
{
    public int camID;
    public ReplayObject replayObject;
    public bool saved;
    public ReplayStorage storage;
    public ReplayRecordOperation recordOp;
    public ReplayPlaybackOperation playbackOp;

    // Constructor
    public CameraReplay(
        int id,
        ReplayObject replayObj,
        bool isSaved,
        ReplayStorage replayStorage
        )
    {
        camID = id;
        replayObject = replayObj;
        saved = isSaved;
        storage = replayStorage;
    }

    // Starts a recording operation
    public void StartRecording()
    {
        // Record if save not set or no current recording exists
        if (!saved || storage.Duration == 0)
        {
            recordOp = ReplayManager.BeginRecording(storage, replayObject, true);
        }
    }

    // Ends the active recording operation
    public void EndRecording()
    {
        if (!recordOp.IsDisposed)
        {
            if (recordOp.IsRecording)
            {
                recordOp.StopRecording();
            }
        }
    }

    // Begins playback of a recording
    public void StartPlayback()
    {
        playbackOp = ReplayManager.BeginPlayback(storage,replayObject);
    }

    // Ends playback operation
    public void EndPlayback()
    {
        if (!playbackOp.IsDisposed)
        {
            if (playbackOp.IsReplaying)
            {
                playbackOp.StopPlayback();
            }
        }

    }
}