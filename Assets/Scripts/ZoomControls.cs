using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages camera zoom and handles zoom button visuals
public class ZoomControls : MonoBehaviour
{
    private float initialFOV = 60;
    private float btnAngle = 11;

    private bool zoomingIn = false;
    private bool zoomingOut = false;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float maxZoom;

    [SerializeField]
    private float zoomSpeed;

    // Initialise state on enable
    void OnEnable()
    {
        // Set the initial zoom states to false
        zoomingIn = false;
        zoomingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if zooming in or out and set the camera FOV and clamp between min and max
        if(zoomingOut) cam.fieldOfView = Mathf.Clamp(
            Mathf.MoveTowards(cam.fieldOfView,
            initialFOV,
            zoomSpeed * Time.deltaTime),
            initialFOV - maxZoom,
            initialFOV );
        if(zoomingIn) cam.fieldOfView = Mathf.Clamp(
            Mathf.MoveTowards(cam.fieldOfView,
            initialFOV - maxZoom,
            zoomSpeed * Time.deltaTime),
            initialFOV - maxZoom,
            initialFOV);
    }

    // Zoom Out button functionality
    // Sets zoom state and handles button visuals
    public void ZoomOut()
    {
        if (!zoomingOut) gameObject.transform.Rotate(0f, btnAngle, 0);
        zoomingOut = true;
        zoomingIn = false;

    }

    // Zoom In button functionality
    // Sets zoom state and handles button visuals
    public void ZoomIn()
    {
        if (!zoomingIn) gameObject.transform.Rotate(0f, -btnAngle, 0);
        zoomingIn = true;
        zoomingOut = false;

    }

    // Resets the Button position to the initial position and resets the zoom state
    public void ResetBtn()
    {
        if (zoomingIn)
        {
            gameObject.transform.Rotate(0f, btnAngle, 0);
        }
        if (zoomingOut)
        {
            gameObject.transform.Rotate(0f, -btnAngle, 0);
        }
        zoomingIn = false;
        zoomingOut = false;
    }
}
