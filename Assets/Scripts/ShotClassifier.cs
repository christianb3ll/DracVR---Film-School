using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Manages classifying shot types
public class ShotClassifier : MonoBehaviour
{
    public enum ShotType
    {
        WideShot,
        TwoShot,
        MidShot,
        MedCloseUp,
        CloseUp,
        ExCloseUp,
        OverShoulder,
        Unframed,
        Unknown
    }

    public Camera cam;

    private ShotType shotType;

    public TextMeshPro textTest;

    public GameObject harker;
    public GameObject dracula;

    private MarkerManager harkerMarkers;
    private MarkerManager draculaMarkers;

    private CharacterFramer draculaFramer;
    private CharacterFramer harkerFramer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the markers from each character
        harkerMarkers = harker.GetComponent<MarkerManager>();
        draculaMarkers = dracula.GetComponent<MarkerManager>();

        harkerFramer = new CharacterFramer();
        draculaFramer = new CharacterFramer();

        harkerFramer.InitialiseCharacter();
        draculaFramer.InitialiseCharacter();
}

    // Update is called once per frame
    void Update()
    {
        // determine if the characters are facing the camera
        harkerFramer.facingCamera = SetFacingCamera(harker.transform);
        draculaFramer.facingCamera = SetFacingCamera(dracula.transform);

        // determine if the characters are obscured by another object
        harkerFramer.obscured = Physics.Linecast(harkerMarkers.GetWaistMarker().position, cam.transform.position);
        draculaFramer.obscured = Physics.Linecast(draculaMarkers.GetWaistMarker().position, cam.transform.position);

        // sets the current visible markers
        SetVisibleMarkers(harkerFramer, harkerMarkers);
        SetVisibleMarkers(draculaFramer, draculaMarkers);

        // determine the shot type for each character
        harkerFramer.shotType = GetCharacterShotType(harkerFramer);
        draculaFramer.shotType = GetCharacterShotType(draculaFramer);

        // determines shot type based on both characters
        shotType = GetShotType(harkerFramer.shotType, draculaFramer.shotType);

        // sets text for debug purposes
        SetText();
    }

    // set the current visible markers for a character
    private void SetVisibleMarkers(CharacterFramer character, MarkerManager markers)
    {
        character.boundsVisible = SetVisible(markers.GetBoundsMarker());
        character.headVisible = SetVisible(markers.GetHeadMarker());
        character.neckVisible = SetVisible(markers.GetNeckMarker());
        character.waistVisible = SetVisible(markers.GetWaistMarker());
        character.footVisible = SetVisible(markers.GetFootMarker());
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

    

    // sets the type of shot for the camera
    private ShotType GetShotType(ShotType shot1, ShotType shot2)
    {
        ShotType shot;

        // Check if either character is in close up
        if(shot1 == ShotType.CloseUp || shot2 == ShotType.CloseUp)
        {
            shot = ShotType.CloseUp;
        }
        // Check if either character is in medium close up
        else if(shot1 == ShotType.MedCloseUp || shot2 == ShotType.MedCloseUp)
        {
            shot = ShotType.MedCloseUp;
        }
        // Check if both characters are in Mid Shot, making a Two shot
        else if (shot1 == ShotType.MidShot && shot2 == ShotType.MidShot)
        {
            shot = ShotType.TwoShot;
        }
        // Check if at least one character is in mid shot
        else if (shot1 == ShotType.MidShot || shot2 == ShotType.MidShot)
        {
            shot = ShotType.MidShot;
        }
        // Check if either is in wideshot
        else if (shot1 == ShotType.WideShot || shot2 == ShotType.WideShot)
        {
            shot = ShotType.WideShot;
        } else
        {
            shot = ShotType.Unknown;
        }

        return shot;
    }

    private ShotType GetCharacterShotType(CharacterFramer character)
    {
        ShotType shot = ShotType.Unknown;

        // Cherck if character is in frame at all
        if (character.headVisible)
        {
            // Check if the character is obscured
            if (!character.obscured)
            {
                // check facing camera
                if (character.facingCamera)
                {
                    // Wide shot
                    if (character.footVisible)
                    {
                        shot = ShotType.WideShot;
                    }
                    // Mid Shot
                    else if (character.waistVisible && !character.boundsVisible)
                    {
                        shot = ShotType.MidShot;
                    }
                    // Medium Close-Up
                    else if (character.neckVisible && !character.boundsVisible && !character.waistVisible)
                    {
                        shot = ShotType.MedCloseUp;
                    }
                    // Close-Up
                    else if (!character.neckVisible && !character.boundsVisible && !character.waistVisible)
                    {
                        shot = ShotType.CloseUp;
                    }
                    else
                    // Unknown shot
                    {
                        shot = ShotType.Unknown;
                    }
                }
                else
                // Character is not facing the camera
                {
                    // Check for reverse wide shot
                    if (character.footVisible)
                    {
                        shot = ShotType.WideShot;
                    }
                    else
                    {
                        shot = ShotType.Unknown;
                    }
                }
            }
        } else
        {
            shot = ShotType.Unframed;
        }

        
        return shot;
    }

    // Sets the text for the current shot type - for DEBUG purposess
    public void SetText()
    {
        string shotName = "";
        
        switch (shotType)
        {
            case ShotType.WideShot :
                shotName = "Wide Shot";
                break;
            case ShotType.TwoShot:
                shotName = "Two Shot";
                break;
            case ShotType.MidShot:
                shotName = "Mid Shot";
                break;
            case ShotType.MedCloseUp:
                shotName = "Medium Close-Up";
                break;
            case ShotType.CloseUp:
                shotName = "Close-Up";
                break;
            case ShotType.Unknown:
                shotName = "Unknown";
                break;
            default:
                shotName = "Unknown";
                break;
        }

        textTest.text = shotName;
    }
}


public class CharacterFramer
{
    public bool facingCamera { get; set; }
    public bool obscured { get; set; }

    public bool boundsVisible { get; set; }
    public bool headVisible { get; set; }
    public bool neckVisible { get; set; }
    public bool waistVisible { get; set; }
    public bool footVisible { get; set; }

    public ShotClassifier.ShotType shotType { get; set; }

    public void InitialiseCharacter()
    {
        facingCamera = false;
        obscured = false;
        boundsVisible = false;
        headVisible = false;
        neckVisible = false;
        waistVisible = false;
        footVisible = false;
    }
}