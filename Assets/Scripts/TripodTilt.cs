using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripodTilt : MonoBehaviour
{
    public GameObject tripodTilt;
    public GameObject tripodVisual;

    public void SetTripodTilt()
    {
        tripodVisual.transform.localEulerAngles = new Vector3(
            tripodTilt.transform.localEulerAngles.x,
            0,
            0);
    }
}
