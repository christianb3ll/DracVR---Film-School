using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

public class SceneManager : MonoBehaviour
{
    // ENUM describing current scene playback state
    private enum SceneState
    {
        Playback,
        Recording,
        Paused,
        Stopped
    }

    private SceneState currentState;

    // Objects and Stand-ins
    public GameObject harker;
    public GameObject harkerStandin;

    public GameObject dracula;
    public GameObject draculaStandin;

    // Layers to manage object/camera visibility
    int hideMainCameraLayer;
    int hideSceneCameraLayer;
    int hideAllCameraLayer;
    int defaultLayer;

    // Timeline
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    private bool recordingExists;
    
    // Camera Materials
    public Material[] camMaterials;

    public MeshRenderer screen;

    private CameraMarker startMarker;

    private List<CameraMarker> markers = new();
    // need a temp list in case of aborted recording

    private Queue<CameraMarker> markerQueue;

    // DEBUG
    public TextMeshPro lengthText;
    public TextMeshPro setText;
    public GameObject testCube;
    public Material red;
    public Material green;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise the layers
        defaultLayer = 0;
        hideMainCameraLayer = LayerMask.NameToLayer("HideMainCamera");
        hideSceneCameraLayer = LayerMask.NameToLayer("HideSceneCameras");
        hideAllCameraLayer = LayerMask.NameToLayer("HideAllCameras");

        // Prevent playback until the user has recorded a scene
        recordingExists = false;
        currentState = SceneState.Stopped;
    }

    // Update is called once per frame
    void Update()
    {
        // other states necessary?
        switch (currentState)
        {
            case SceneState.Stopped :
                break;
            case SceneState.Paused:
                break;
            case SceneState.Playback :
                if(markerQueue.Count > 0)
                {
                    // Set the next mark in the queue
                    CameraMarker nextMark = markerQueue.Peek();

                    // Check if current time has passed the time of the next marker
                    if (playableDirector.time >= nextMark.timestamp)
                    {
                        // dequeue the marker
                        CameraMarker marker = markerQueue.Dequeue();
                        // set the active camera to the current marker
                        SetCamera(marker.camID);
                    }
                }
                break;
            case SceneState.Recording:

                break;
            default:

                break;

        }
        
    }

    // Sets up playback for the scene
    public void StartPlayback()
    {
        // don't start playback unless a recording has bee made
        if (recordingExists)
        {
            // Hide the animated characters from the live environment
            SetGameLayerRecursive(harker, hideMainCameraLayer);
            SetGameLayerRecursive(dracula, hideMainCameraLayer);

            // Hide the standin characters from the scene cameras
            SetGameLayerRecursive(harkerStandin, hideSceneCameraLayer);
            SetGameLayerRecursive(draculaStandin, hideSceneCameraLayer);

            // start the timeline
            playableDirector.Play();
            // set the playback state to playing
            currentState = SceneState.Playback;

            // setup the queue
            markerQueue = new Queue<CameraMarker>(markers);
        }
        
    }

    // ends the playback state
    public void EndPlayback()
    {
        if(playableDirector.state == PlayState.Playing && currentState == SceneState.Playback)
        {
            // Return the characters to the default layer
            SetGameLayerRecursive(harker, defaultLayer);
            SetGameLayerRecursive(dracula, defaultLayer);

            // Hide standins from all cameras
            SetGameLayerRecursive(harkerStandin, hideAllCameraLayer);
            SetGameLayerRecursive(draculaStandin, hideAllCameraLayer);

            // Reset the scene
            ResetPlayback();
            // set playback to stopped state
            currentState = SceneState.Stopped;
            // clear the marker queue
            markerQueue.Clear();
        }
        
    }

    // handles Stop button on the console
    public void StopBtn()
    {
        // check the current state
        switch (currentState)
        {
            case SceneState.Playback:
                EndPlayback();
                break;
            case SceneState.Paused:
                EndPlayback();
                break;
            case SceneState.Recording:
                AbortRecording();
                break;
            default:

                break;

        }
    }

    // Starts the cene in recording state
    public void StartRecording()
    {
        // Only start trecording from sstopped state
        if(currentState == SceneState.Stopped)
        {
            // start the scene
            playableDirector.Play();
            // set the state to recording
            currentState = SceneState.Recording;
        }
    }

    // Ends the recording. Called when a recording fully completes without abort
    public void EndRecording()
    {
        // Ensure we are recording
        if(currentState == SceneState.Recording)
        {        
            recordingExists = true;
            // set the playback state to stopped
            currentState = SceneState.Stopped;
            // sort the recorded markers
            markers.Sort(SortByTimestamp);
        }
    }

    // Called on cut or stop button press
    public void AbortRecording()
    {
        // check if currently recording
        if (currentState == SceneState.Recording)
        {
            // reset the playback state
            ResetPlayback();
            // set the playback state to stopped
            currentState = SceneState.Stopped;
        }
            
    }

    // Resets the playback of the scene to frame 0 and stops the playback
    private void ResetPlayback()
    {
        playableDirector.time = 0;
        playableDirector.Evaluate();
        playableDirector.Stop();
        
    }

    // Sorting function that allows sorting the marker list according to timestamp
    private static int SortByTimestamp(CameraMarker marker1, CameraMarker marker2)
    {
        return marker1.timestamp.CompareTo(marker2.timestamp);
    }

    // recursive function to set the layer for an object and its children
    // based on the example:
    // https://forum.unity.com/threads/help-with-layer-change-in-all-children.779147/
    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameLayerRecursive(child.gameObject, layer);
        }
    }

    // Sets a marker at the current timestamp for a given camera ID
    public void SetMarker(int id)
    {
        // check that we are recording
        if (currentState == SceneState.Recording)
        {
            // Setup the marker object
            CameraMarker marker = new CameraMarker();
            // set the timestamp to the current time
            marker.timestamp = playableDirector.time;

            marker.camID = id;

            // Add the new marker to the list
            markers.Add(marker);

            // Set the current camera
            SetCamera(id);

            // DEBUG TEXT
            setText.text = "Marker " + id + " added at " + playableDirector.time;
            lengthText.text = markers.Count.ToString();
        }
        // If not playing set the starting camera 
        else if(currentState == SceneState.Stopped)
        {
            startMarker = new CameraMarker();
            startMarker.timestamp = 0;
            startMarker.camID = id;

            SetCamera(id);
        }
    }

    // Sets the given camera ID to display on the monitor
    private void SetCamera(int id)
    {
        // Change the material for the screen
        // -1 to convert cameraID to array int
        screen.material = camMaterials[id - 1];
    }
}

// Camera marker class
public class CameraMarker : MonoBehaviour
{
    public double timestamp { get; set; }
    public int camID { get; set; }
}
