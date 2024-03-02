using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the playback of audio from the timeline
public class AudioManager : MonoBehaviour
{
    public AudioSource[] sceneAudio;

    public AudioSource monitorAudio;

    // Sets timeline audio to playback through the monitor
    public void EnableMonitorAudio()
    {
        foreach(AudioSource source in sceneAudio)
        {
            source.enabled = false;
        }

        monitorAudio.enabled = true;
    }

    // Sets timeline audio to play in scene
    public void EnableSceneAudio()
    {
        foreach (AudioSource source in sceneAudio)
        {
            source.enabled = true;
        }

        monitorAudio.enabled = false;
    }
}
