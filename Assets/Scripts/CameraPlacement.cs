using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraPlacement : MonoBehaviour
{
    public GameObject[] cameraObjects;
    public GameObject tripodPrefab;

    public GameObject initCamTransform;

    public float cameraOffset;

    private bool[] activeCameras = new bool[5];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < activeCameras.Length; i++)
        {
            DeactivateCamera(i);
        }

        ActivateCamera(0);
    }

    // Method for Activating a camera by ID
    private void ActivateCamera(int camID)
    {
        activeCameras[camID] = true;
        cameraObjects[camID].SetActive(true);
    }

    // Method for Deactivating a camera by ID
    private void DeactivateCamera(int camID)
    {
        activeCameras[camID] = false;
        cameraObjects[camID].SetActive(false);
    }

    // Places a camera at the desired location
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

            Transform userPos = Camera.main.transform;
            Vector3 userDir = Camera.main.transform.forward;

            // Instantiate a tripod at the user's location
            GameObject tripod = Instantiate(
                tripodPrefab,
                new Vector3(userPos.position.x, 0, userPos.position.z),
                Quaternion.LookRotation(userPos.forward, Vector3.up));

            // Set the tilt of the tripod
            Transform tripodTilt = tripod.transform.Find("PanHandle/PanTransform/TiltHandle/HandleGrip");

            // Set the camera as the target object for the tripod socket interactor
            tripod.GetComponent<XRSocketInteractor>().StartManualInteraction(cameraObjects[camID].GetComponent<IXRSelectInteractable>());

        }
    }
}
