using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraArea : MonoBehaviour
{
    // Setup the interactor
    XRBaseInteractable interactor;
    

    // Start is called before the first frame update
    void Start()
    {
        interactor = gameObject.GetComponent<XRSimpleInteractable>();
        //interactor.GetAttachTransform()
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if selected
        // get user input and update camera rotatio
    }

    // On Select
    // Create a camera reticle
    // freeze or hide ray
    //

    // On deselect
    // re-enable the ray
    // destroy the reticle

    // On activate
    // https://forum.unity.com/threads/grab-interactable-anchor-control-without-motion-input.1540358/

}
