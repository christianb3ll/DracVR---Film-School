using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the marker transforms for a character and provides Get methods
public class MarkerManager : MonoBehaviour
{
    public Transform boundsMarker;
    public Transform headMarker;
    public Transform neckMarker;
    public Transform waistMarker;
    public Transform footMarker;

    // Returns the transform for the Bounds marker
    public Transform GetBoundsMarker()
    {
        return boundsMarker;
    }

    // Returns the transform for the Head marker
    public Transform GetHeadMarker()
    {
        return headMarker;
    }

    // Returns the transform for the Neck marker
    public Transform GetNeckMarker()
    {
        return neckMarker;
    }

    // Returns the transform for the Waist marker
    public Transform GetWaistMarker()
    {
        return waistMarker;
    }

    // Returns the transform for the Foot marker
    public Transform GetFootMarker()
    {
        return footMarker;
    }

}
