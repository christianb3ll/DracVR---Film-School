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
        switch (currentState)
        {
            case SceneState.Stopped :
                break;
            case SceneState.Playback :
                if(markerQueue.Count > 0)
                {
                    testCube.GetComponent<MeshRenderer>().material = red;
                    CameraMarker nextMark = markerQueue.Peek();
                    if (playableDirector.time >= nextMark.timestamp)
                    {
                        
                        testCube.GetComponent<MeshRenderer>().material = green;
                        CameraMarker marker = markerQueue.Dequeue();
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
        if (recordingExists)
        {
            // Hide the animated characters from the live environment
            SetGameLayerRecursive(harker, hideMainCameraLayer);
            SetGameLayerRecursive(dracula, hideMainCameraLayer);

            // Hide the standin characters from the scene cameras
            SetGameLayerRecursive(harkerStandin, hideSceneCameraLayer);
            SetGameLayerRecursive(draculaStandin, hideSceneCameraLayer);

            playableDirector.Play();
            currentState = SceneState.Playback;

            // setup the queue
            markerQueue = new Queue<CameraMarker>(markers);
        }
        
    }

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
            currentState = SceneState.Stopped;
            markerQueue.Clear();
        }
        
    }

    public void StopBtn()
    {
        switch (currentState)
        {
            case SceneState.Playback:
                EndPlayback();
                break;
            case SceneState.Recording:
                AbortRecording();
                break;
            default:

                break;

        }
    }

    public void StartRecording()
    {
        if(currentState == SceneState.Stopped)
        {
            playableDirector.Play();
            currentState = SceneState.Recording;
        }
    }

    public void EndRecording()
    {
        if(currentState == SceneState.Recording)
        {
            if (!recordingExists) recordingExists = true;
            currentState = SceneState.Stopped;
            markers.Sort(SortByTimestamp);
        }
    }

    public void AbortRecording()
    {
        if (currentState == SceneState.Recording)
        {
            ResetPlayback();
            currentState = SceneState.Stopped;
        }
            
    }

    // Resets the playback of the scene to frame 0
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

    // https://forum.unity.com/threads/help-with-layer-change-in-all-children.779147/
    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameLayerRecursive(child.gameObject, layer);
        }
    }

    public void SetMarker(int id)
    {
        if (currentState == SceneState.Recording)
        {
            // Setup the marker object
            CameraMarker marker = new CameraMarker();
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
    }

    private void SetCamera(int id)
    {
        // Change the material for the screen
        // -1 to convert cameraID to array int
        screen.material = camMaterials[id - 1];
    }
}

public class CameraMarker : MonoBehaviour
{
    public double timestamp { get; set; }
    public int camID { get; set; }
}
