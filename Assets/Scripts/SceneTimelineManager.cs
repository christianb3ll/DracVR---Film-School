using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

// Manages scene playback and scene related methods
public class SceneTimelineManager : MonoBehaviour
{
    // ENUM describing current scene playback state
    private enum SceneState
    {
        Playback,
        Live,
        Stopped
    }

    private SceneState currentState;

    // Objects and Stand-ins
    public GameObject harker;
    public GameObject harkerStandin;

    public GameObject dracula;
    public GameObject draculaStandin;

    public GameObject bats;
    public GameObject briefcase;

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

    // Black and white camera materials
    public Material[] camBWMaterials;

    private bool isBlackAndWhite;

    private int activeCamera;
    public MeshRenderer screen;

    private bool recordMarkers;
    private CameraMarker startMarker;

    private List<CameraMarker> markers = new();

    private Queue<CameraMarker> markerQueue;

    public TextMeshPro consoleLogText;

    public CameraReplays replays;

    public UnityEvent SceneStartEvents;
    public UnityEvent SceneEndEvents;

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

        // Sets Black and white to false
        isBlackAndWhite = false;

        // Set the current camera as inactive
        activeCamera = -1;

        // Set recording markers to false
        recordMarkers = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == SceneState.Playback)
        {
            if (markerQueue.Count > 0)
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
        }        
    }

    // Checks if the scene is in the stopped state
    public bool SceneStopped()
    {
        return currentState == SceneState.Stopped;
    }

    // Checks if the scene is in the live state
    public bool SceneLive()
    {
        return currentState == SceneState.Live;
    }

    // Sets up playback for the scene
    public void StartPlayback()
    {
        // don't start playback unless a recording has bee made
        if (recordingExists)
        {
            SceneStartEvents.Invoke();
            // Hide the animated characters from the live environment
            SetGameLayerRecursive(harker, hideMainCameraLayer);
            SetGameLayerRecursive(dracula, hideMainCameraLayer);
            SetGameLayerRecursive(bats, hideMainCameraLayer);
            SetGameLayerRecursive(briefcase, hideMainCameraLayer);

            // Hide the standin characters from the scene cameras
            SetGameLayerRecursive(harkerStandin, hideSceneCameraLayer);
            SetGameLayerRecursive(draculaStandin, hideSceneCameraLayer);

            // start the timeline
            playableDirector.Play();
            // set the playback state to playing
            currentState = SceneState.Playback;

            // Starts replays
            //replays.StartPlayback();

            // setup the queue
            markerQueue = new Queue<CameraMarker>(markers);
        } else
        {
            Log("No Recording Exists!");
        }

        
    }

    // starts the live scene
    public void StartLive()
    {
        SceneStartEvents.Invoke();
        // Only start trecording from stopped state
        if (currentState == SceneState.Stopped)
        {
            // start the scene
            playableDirector.Play();
            // set the state to Live
            currentState = SceneState.Live;

            // Starts replay recording
            //replays.StartRecording();
        }
    }

    // ends the scene
    public void EndPlayback()
    {
        if(playableDirector.state == PlayState.Playing)
        {
            if(currentState == SceneState.Playback)
            {
                // Return the characters to the default layer
                SetGameLayerRecursive(harker, defaultLayer);
                SetGameLayerRecursive(dracula, defaultLayer);
                SetGameLayerRecursive(bats, defaultLayer);
                SetGameLayerRecursive(briefcase, defaultLayer);

                // Hide standins from all cameras
                SetGameLayerRecursive(harkerStandin, hideAllCameraLayer);
                SetGameLayerRecursive(draculaStandin, hideAllCameraLayer);

                // clear the marker queue
                markerQueue.Clear();

                if (recordMarkers)
                {
                    // sort the recorded markers
                    markers.Sort(SortByTimestamp);
                }
                // Ends replay playback
                //replays.EndPlayback();
            }

            if (currentState == SceneState.Live)
            {
                // Ends replay recording
                //replays.EndRecording();
                recordingExists = true;
            }

            // Reset the scene
            ResetPlayback();
            // set playback to stopped state
            currentState = SceneState.Stopped;
   
            SceneEndEvents.Invoke();
        }
        
    }

    // Called on cut or stop button press
    public void AbortLive()
    {
        // check if currently recording
        if (currentState == SceneState.Live)
        {
            // reset the playback state
            ResetPlayback();
            // set the playback state to stopped
            currentState = SceneState.Stopped;

            // ends replay recording
            //replays.EndRecording();

            SceneEndEvents.Invoke();
        }
            
    }

    // Resets the playback of the scene to frame 0 and stops the playback
    private void ResetPlayback()
    {
        playableDirector.time = 0;
        playableDirector.Evaluate();
        playableDirector.Stop();
        
    }

    // Toggle recording markers
    public void ToggleMarkerRecording()
    {
        recordMarkers = !recordMarkers;
        if (recordMarkers)
        {
            Log("Recording Markers");
        } else
        {
            Log("Recording Deactivated");
        }
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
        // check that we are playing and deck is set to record markers
        if (currentState == SceneState.Playback && recordMarkers)
        {
            // Setup the marker object
            CameraMarker marker = new CameraMarker();
            // set the timestamp to the current time
            marker.timestamp = playableDirector.time;

            marker.camID = id;

            // Add the new marker to the list
            markers.Add(marker);

            // Log to the in game console
            string logText = "Marker added for camera " + id;
            Log(logText);
        }
        // If not playing set the starting camera 
        else if(currentState == SceneState.Stopped)
        {
            startMarker = new CameraMarker();
            startMarker.timestamp = 0;
            startMarker.camID = id;
        }

        // Set the current camera
        SetCamera(id);
    }

    // Clear all markers
    public void ClearMarkers()
    {
        // Only clear markers if stopped
        if(currentState == SceneState.Stopped)
        {
            // clear all markers
            markers.Clear();
            Log("Markers cleared");
        }
    }

    // Toggles black and white and colour
    public void ToggleBlackAndWhite()
    {
        isBlackAndWhite = !isBlackAndWhite;

        if(activeCamera != -1)
        {
            SetScreenMaterial(activeCamera);
        }
    }

    // Sets the given camera ID to display on the monitor
    private void SetCamera(int id)
    {
        SetScreenMaterial(id);

        activeCamera = id;
        Log("Set to Camera " + id);
        
    }

    // Set the screen material
    private void SetScreenMaterial(int id)
    {
        // Check if black and white
        if (!isBlackAndWhite)
        {
            // Change the material for the screen
            // -1 to convert cameraID to array int
            screen.material = camMaterials[id - 1];
        }
        else
        {
            screen.material = camBWMaterials[id - 1];
        }
    }

    public void Evaluate()
    {
        Log("Evaluate functionality coming soon");
    }

    // Logs messsages to the in-game console 
    private void Log(string logText)
    {
        consoleLogText.text = logText;
    }

}

// Camera marker class
public class CameraMarker
{
    public double timestamp { get; set; }
    public int camID { get; set; }
}
