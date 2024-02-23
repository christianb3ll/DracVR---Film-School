using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages the clapperboard interactable
public class Clapperboard : MonoBehaviour
{
    private AudioSource audioSource;

    public SceneManager sceneManager;

    // Events for start and stop scene
    public UnityEvent StartEvent;
    public UnityEvent StopEvent;

    // Start is called before the first frame update
    void Start()
    {
        // get the audio source from the clapperboard object
        audioSource = GetComponent<AudioSource>();
    }

    // Called when the user activates the interactable
    public void OnClap()
    {
        // Check the state of the scene and invoke appropriate events
        if (sceneManager.SceneStopped())
        {
            StartEvent.Invoke();
        } else
        {
            StopEvent.Invoke();
        }
    }

    // Play the clap sound - called in animation
    public void PlayAudioClip()
    {
        audioSource.Play();
    }
}
