using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles functions for responding to voice commands
public class VoiceCommandManager : MonoBehaviour
{

    public GameObject blockingPreview;
    public CameraManager cameraManager;
    public Tablet tabletHints;

    // Handles object placement voice commands
    public void PlaceObject(string[] response)
    {
        // Check we're getting a response
        if(response.Length > 0)
        {
            // Check if object to be placed is a tripod or camera
            if(response[0].Equals("tripod",System.StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("Success Tripod");
                cameraManager.PlaceTripod();
            } else if (response[0].Equals("camera", System.StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("Success camera");
                cameraManager.PlaceCamera();
            }
        }
    }

    // Handles blocking togle commands
    public void ToggleBlocking(string[] response)
    {
        // Check we're getting a response
        if (response.Length > 0)
        {
            // Check if received command is show/hide
            if(response[0].Equals("show", System.StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("show the blocking");
                blockingPreview.SetActive(true);
            } else if(response[0].Equals("hide", System.StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("hide the blocking");
                blockingPreview.SetActive(false);
            }
        }
    }

    // Handles hint canvas voice commands
    public void ToggleHints(string[] response)
    {
        // Check we're getting a response
        if (response.Length > 0)
        {
            // Check if received command is show/hide
            if (response[0].Equals("show", System.StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("show hints");
                tabletHints.ToggleHints();
            }
            else if (response[0].Equals("hide", System.StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("hide hints");
                tabletHints.ToggleHints();
            }
        }
    }

    // Handles camera visibility voice commands
    public void HideCamera(string[] response)
    {
        // Check we're getting a response
        if (response.Length > 0)
        {
            // initialise camID to null value
            int camID = -1;
            int.TryParse(response[0], out camID);

            // if camID is valid
            if(camID >=1 && camID <= 5)
            {
                // adjust camera ID to array value
                camID--;

                // deactivate the camera
                cameraManager.DeactivateCamera(camID);
            }
           
        }
    }
}
