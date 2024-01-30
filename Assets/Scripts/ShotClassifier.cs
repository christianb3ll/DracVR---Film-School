using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private bool harkerBoundsVisible;
    private bool harkerHeadVisible;
    private bool harkerNeckVisible;
    private bool harkerWaistVisible;
    private bool harkerFootVisible;

    // Start is called before the first frame update
    void Start()
    {
        harkerMarkers = harker.GetComponent<MarkerManager>();

        harkerBoundsVisible = false;
        harkerHeadVisible = false;
        harkerNeckVisible = false;
        harkerWaistVisible = false;
        harkerFootVisible = false;
}

    // Update is called once per frame
    void Update()
    {
        harkerFacingCamera = SetFacingCamera(harker.transform);
        SetVisibleMarkers(harkerMarkers);

        SetShotType(
            harkerFacingCamera,
            harkerBoundsVisible,
            harkerHeadVisible,
            harkerNeckVisible,
            harkerWaistVisible,
            harkerFootVisible);
        SetText();
    }

    private void SetVisibleMarkers(MarkerManager character)
    {
        harkerBoundsVisible = SetVisible(character.GetBoundsMarker());
        harkerHeadVisible = SetVisible(character.GetHeadMarker());
        harkerNeckVisible = SetVisible(character.GetNeckMarker());
        harkerWaistVisible = SetVisible(character.GetWaistMarker());
        harkerFootVisible = SetVisible(character.GetFootMarker());
    }

    private bool SetVisible(Transform marker)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(marker.position);
        return ((viewPos.x > 0F && viewPos.x < 1F) && (viewPos.y > 0F && viewPos.y < 1F));
    }

    private bool SetFacingCamera(Transform character)
    {
        return (Vector3.Dot(character.forward, cam.transform.position - character.position) > 0);
    }

    private void SetShotType(bool facingCamera, bool boundsVisible, bool headVisible, bool neckVisible, bool waistVisible, bool footVisible)
    {
        if (facingCamera)
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

    public void SetText()
    {
        textTest.text = shotType;
    }
}
