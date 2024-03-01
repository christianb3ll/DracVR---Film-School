using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle the position and availability of interactabless
public class InteractableManager : MonoBehaviour
{
    public GameObject viewfinder;
    public Transform viewfinderOrigin;

    public GameObject clapperboard;
    public Transform clapperboardOrigin;

    public GameObject megaphone;
    public Transform megaphoneOrigin;

    // Resets the position of interactables to the given orgin points
    public void ResetObjects()
    {
        viewfinder.transform.position = viewfinderOrigin.position;
        viewfinder.transform.rotation = viewfinderOrigin.rotation;

        clapperboard.transform.position = clapperboardOrigin.position;
        clapperboard.transform.rotation = clapperboardOrigin.rotation;

        megaphone.transform.position = megaphoneOrigin.position;
        megaphone.transform.rotation = megaphoneOrigin.rotation;
    }
}
