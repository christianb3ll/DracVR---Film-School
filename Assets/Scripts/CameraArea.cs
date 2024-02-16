using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraArea : MonoBehaviour
{
    // Setup the interactor
    XRSimpleInteractable interactor;

    public XRRayInteractor cameraRay;

    public GameObject testObj;

    // Start is called before the first frame update
    void Start()
    {
        interactor = gameObject.GetComponent<XRSimpleInteractable>();
        //interactor.GetAttachTransform()


    }

    // Update is called once per frame
    void Update()
    {
        if (interactor.isHovered)
        {
            RaycastHit rayHit;
            cameraRay.TryGetCurrent3DRaycastHit(out rayHit);
            if (rayHit.colliderInstanceID == gameObject.GetInstanceID())
            {
                Debug.Log("Got the ID");
            }
            Vector3 cameraPos = rayHit.point;
            testObj.transform.position = cameraPos;
         
        }
        // check if selected
        // get user input and update camera rotatio
    }

    public void TestSelect()
    {

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

    // TryGetCurrent3DRaycastHit()


    public void PlaceCamera()
    {
        RaycastHit rayHit;
        cameraRay.TryGetCurrent3DRaycastHit(out rayHit);

        Transform cameraPos = rayHit.transform;

        Instantiate(testObj, cameraPos.position, Quaternion.identity);
    }

}
