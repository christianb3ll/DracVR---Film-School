using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomControls : MonoBehaviour
{
    private float initialFOV = 60;
    private float btnAngle = 11;

    private bool zoomingIn;
    private bool zoomingOut;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float maxZoom;

    [SerializeField]
    private float zoomSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial zoom states to false
        zoomingIn = false;
        zoomingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if zooming in or out and set the camera FOV
        if(zoomingOut) cam.fieldOfView = Mathf.Clamp(Mathf.MoveTowards(cam.fieldOfView, initialFOV, zoomSpeed * Time.deltaTime), initialFOV - maxZoom, initialFOV );
        if(zoomingIn) cam.fieldOfView = Mathf.Clamp(Mathf.MoveTowards(cam.fieldOfView, initialFOV - maxZoom, zoomSpeed * Time.deltaTime), initialFOV - maxZoom, initialFOV);
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
