using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fixes an issue relating to tilt angle in XRJoystick class
public class TripodTilt : MonoBehaviour
{
    public GameObject tripodTilt;
    public GameObject tripodVisual;

    // Sets the tilt angle of the tripod head
    public void SetTripodTilt()
    {
        tripodVisual.transform.localEulerAngles = new Vector3(
            tripodTilt.transform.localEulerAngles.x,
            0,
            0);
    }
}
