using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Manages classifying shot types
public class ShotClassifier : MonoBehaviour
{
    public Camera cam;

    private string shotType;

    public TextMeshPro textTest;

    public GameObject harker;
    public GameObject dracula;

    private MarkerManager harkerMarkers;
    private MarkerManager draculaMarkers;

    private bool harkerFacingCamera;
    private bool harkerObscured;

    private bool draculaFacingCamera;
    private bool draculaObscured;

    private bool harkerBoundsVisible;
    private bool harkerHeadVisible;
    private bool harkerNeckVisible;
    private bool harkerWaistVisible;
    private bool harkerFootVisible;

    private bool draculaBoundsVisible;
    private bool draculaHeadVisible;
    private bool draculaNeckVisible;
    private bool draculaWaistVisible;
    private bool draculaFootVisible;

    // Start is called before the first frame update
    void Start()
    {
        // Get the markers from each character
        harkerMarkers = harker.GetComponent<MarkerManager>();
        draculaMarkers = dracula.GetComponent<MarkerManager>();

        // initialise Harker markers
        harkerBoundsVisible = false;
        harkerHeadVisible = false;
        harkerNeckVisible = false;
        harkerWaistVisible = false;
        harkerFootVisible = false;

        // initialise Dracula markers
        draculaBoundsVisible = false;
        draculaHeadVisible = false;
        draculaNeckVisible = false;
        draculaWaistVisible = false;
        draculaFootVisible = false;
}

    // Update is called once per frame
    void Update()
    {
        // determine if the characters are facing the camera
        harkerFacingCamera = SetFacingCamera(harker.transform);
        draculaFacingCamera = SetFacingCamera(dracula.transform);

        // determine if the characters are obscured by another object
        harkerObscured = Physics.Linecast(harkerMarkers.GetWaistMarker().position, cam.transform.position);
        draculaObscured = Physics.Linecast(draculaMarkers.GetWaistMarker().position, cam.transform.position);

        // sets the current visible markers
        SetVisibleMarkers(harkerMarkers);
        SetVisibleMarkers(draculaMarkers);

        // determine the shot type
        SetShotType(
            harkerFacingCamera,
            harkerBoundsVisible,
            harkerHeadVisible,
            harkerNeckVisible,
            harkerWaistVisible,
            harkerFootVisible);
        // set dracula shot type and determine combined shots

        // sets text for debug purposes
        SetText();
    }

    // set the current visible markers for a character
    private void SetVisibleMarkers(MarkerManager character)
    {
        harkerBoundsVisible = SetVisible(character.GetBoundsMarker());
        harkerHeadVisible = SetVisible(character.GetHeadMarker());
        harkerNeckVisible = SetVisible(character.GetNeckMarker());
        harkerWaistVisible = SetVisible(character.GetWaistMarker());
        harkerFootVisible = SetVisible(character.GetFootMarker());
    }

    // sets the visibility of an individual marker
    private bool SetVisible(Transform marker)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(marker.position);
        return ((viewPos.x > 0F && viewPos.x < 1F) && (viewPos.y > 0F && viewPos.y < 1F));
    }

    // sets whether a character is facing the camera or ot
    private bool SetFacingCamera(Transform character)
    {
        return (Vector3.Dot(character.forward, cam.transform.position - character.position) > 0);
    }

    // sets the type of shot
    private void SetShotType(bool facingCamera, bool boundsVisible, bool headVisible, bool neckVisible, bool waistVisible, bool footVisible)
    {
        if (facingCamera && !harkerObscured)
        {
            if (headVisible && footVisible)
            {
                shotType = "Wide Shot";
            }
            else if (headVisible && waistVisible & !boundsVisible)
            {
                shotType = "Mid Shot";
            }
            else if (headVisible && neckVisible && !boundsVisible && !waistVisible)
            {
                shotType = "Close Up";
            }
            else
            {
                shotType = "Unknown";
            }
        }
        else
        {
            shotType = "?";
        }
    }

    // Sets the text for the current shot type - for DEBUG purposess
    public void SetText()
    {
        textTest.text = shotType;
    }
}
