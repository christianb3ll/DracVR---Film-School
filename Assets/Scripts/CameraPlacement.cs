using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Managed the placement of cameras
public class CameraPlacement : MonoBehaviour
{
    public GameObject[] cameraObjects;
    public GameObject tripodPrefab;

    public GameObject initCamTransform;

    public float cameraOffset;

    private bool[] activeCameras = new bool[5];

    // Dissable all cameras on Awake
    void Awake()
    {
        for(int i = 0; i < activeCameras.Length; i++)
        {
            DeactivateCamera(i);
        }
        // reacctivate the first camera
        ActivateCamera(0);
    }

    // Method for Activating a camera by ID
    public void ActivateCamera(int camID)
    {
        activeCameras[camID] = true;
        cameraObjects[camID].SetActive(true);
    }

    // Method for Deactivating a camera by ID
    public void DeactivateCamera(int camID)
    {
        activeCameras[camID] = false;
        cameraObjects[camID].SetActive(false);
    }

    // Method for checking if cameras can be placed
    public bool CamerasAvailable()
    {
        for (int i = 0; i < activeCameras.Length; i++)
        {
            if (activeCameras[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    // Places a camera at the player location
    public void PlaceCamera()
    {
        // Initialise Cam ID to -1 as null value
        int camID = -1;

        // Check currently active cameras and find the first inactive camera
        for (int i = 0; i < activeCameras.Length; i++)
        {
            if(activeCameras[i] == false && camID == -1)
            {
                camID = i;
                activeCameras[i] = true;
            }
        }

        // Check that camID is not a null value
        if(camID != -1)
        {
            // Activate the camera
            cameraObjects[camID].SetActive(true);

            // initialise position
            Vector3 userPos = Camera.main.transform.position;
            Vector3 userDir = Camera.main.transform.forward;

            // initialise rotation
            Quaternion userRotation = Camera.main.transform.rotation;
            Quaternion cameraRotation = Camera.main.transform.rotation;

            // new possition with camera offset
            Vector3 newPos = userPos + userDir * cameraOffset;

            // calculate new rotations
            userRotation.SetLookRotation(new Vector3(userDir.x, 0, userDir.z), Vector3.up);
            cameraRotation.SetLookRotation(userDir, Vector3.up);

            // Instantiate a tripod at the user's location
            GameObject tripod = Instantiate(
                tripodPrefab, new Vector3(newPos.x, 0, newPos.z) , userRotation);

            // Set the tilt of the tripod
            // Transform tripodTilt = tripod.transform.Find("PanHandle/PanTransform/TiltHandle/HandleGrip");

            // Set the camera as the target object for the tripod socket interactor
            tripod.GetComponent<XRSocketInteractor>().StartManualInteraction(cameraObjects[camID].GetComponent<IXRSelectInteractable>());

        }
    }

    // Places a camera at the specified transform
    public void PlaceCamera(Transform location)
    {
        // Initialise Cam ID to -1 as null value
        int camID = -1;

        // Check currently active cameras and find the first inactive camera
        for (int i = 0; i < activeCameras.Length; i++)
        {
            if (activeCameras[i] == false && camID == -1)
            {
                camID = i;
                activeCameras[i] = true;
            }
        }

        // Check that camID is not a null value
        if (camID != -1)
        {
            // Activate the camera
            cameraObjects[camID].SetActive(true);

            // Instantiate a tripod at the chosen location
            GameObject tripod = Instantiate(tripodPrefab, location.position, location.rotation);

            // Set the camera as the target object for the tripod socket interactor
            tripod.GetComponentInChildren<XRSocketInteractor>().StartManualInteraction(cameraObjects[camID].GetComponent<IXRSelectInteractable>());
            
        }
    }
}
