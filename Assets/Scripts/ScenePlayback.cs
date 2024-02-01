using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class ScenePlayback : MonoBehaviour
{
    public GameObject harker;
    public GameObject harkerStandin;

    public GameObject dracula;
    public GameObject draculaStandin;

    int hideMainCameraLayer;
    int hideSceneCameraLayer;
    int hideAllCameraLayer;
    int defaultLayer;

    public PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        defaultLayer = 0;
        hideMainCameraLayer = LayerMask.NameToLayer("HideMainCamera");
        hideSceneCameraLayer = LayerMask.NameToLayer("HideSceneCameras");
        hideAllCameraLayer = LayerMask.NameToLayer("HideAllCameras");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Sets up playback for the scene
    public void StartPlayback()
    {
        // Hide the animated characters from the live environment
        SetGameLayerRecursive(harker, hideMainCameraLayer);
        SetGameLayerRecursive(dracula, hideMainCameraLayer);

        // Hide the standin characters from the scene cameras
        SetGameLayerRecursive(harkerStandin, hideSceneCameraLayer);
        SetGameLayerRecursive(draculaStandin, hideSceneCameraLayer);

        playableDirector.Play();
    }

    public void EndPlayback()
    {
        // Return the characters to the default layer
        SetGameLayerRecursive(harker, defaultLayer);
        SetGameLayerRecursive(dracula, defaultLayer);

        // Hide standins from all cameras
        SetGameLayerRecursive(harkerStandin, hideAllCameraLayer);
        SetGameLayerRecursive(draculaStandin, hideAllCameraLayer);

        // Reset the scene
        ResetPlayback();
    }

    // Resets the playback of the scene to frame 0
    private void ResetPlayback()
    {
        playableDirector.time = 0;
        playableDirector.Evaluate();
        playableDirector.Stop();
        
    }

    // https://forum.unity.com/threads/help-with-layer-change-in-all-children.779147/
    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameLayerRecursive(child.gameObject, layer);
        }
    }
}
