using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordControls : MonoBehaviour
{
    [SerializeField]
    UltimateReplay.ReplayObject replayObject;
    [SerializeField]
    UltimateReplay.ReplayTransform replayTransform;

    private bool replayEnabled;
    private CameraReplays cameraReplays;

    // Start is called before the first frame update
    void Start()
    {
        replayObject.enabled = false;
        replayTransform.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPress(){

    }

    public void ToggleCameraReplay()
    {
        if (replayEnabled)
        {
            replayObject.enabled = false;
            replayTransform.enabled = false;
            cameraReplays.RemoveFromScene(gameObject);
        }
        else
        {
            replayObject.enabled = true;
            replayTransform.enabled = true;
            cameraReplays.AddToScene(gameObject);
        }

        replayEnabled = !replayEnabled;
    }
}
