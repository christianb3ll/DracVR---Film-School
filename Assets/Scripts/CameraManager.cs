using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Managed the placement of cameras
public class CameraManager : MonoBehaviour
{
    public GameObject[] cameraObjects;
    public GameObject tripodPrefab;

    public AudioClip tripodAudio;

    public GameObject initCamTransform;

    public float cameraOffset;

    private bool[] activeCameras = new bool[5];
    private bool[] saveRecording = new bool[5];

    public CameraReplays replays;

    // Dissable all cameras on Awake
    void Awake()
    {
        for(int i = 0; i < activeCameras.Length; i++)
        {
            //DeactivateCamera(i);
            activeCameras[i] = false;
            // initialise recording saves to false
            saveRecording[i] = false;
        }
        // reacctivate the first camera
        //ActivateCamera(0);
        activeCameras[0] = true;
    }

    // Checks if a camera is active in the scene
    public bool IsActive(int camID)
    {
        return activeCameras[camID - 1];
    }

    // Method for Activating a camera by ID
    public void ActivateCamera(int camID)
    {
        // Activate the camera
        activeCameras[camID] = true;
        cameraObjects[camID].SetActive(true);
        // Add the camera to the replay scene
        replays.AddToScene(cameraObjects[camID]);
    }

    // Method for Deactivating a camera by ID
    public void DeactivateCamera(int camID)
    {
        activeCameras[camID] = false;
        cameraObjects[camID].SetActive(false);
        // Remove the camera from the replay scene
        replays.RemoveFromScene(cameraObjects[camID]);
    }

    // Activates saving camera replayss for a given camera
    public void ActivateSaveRecording(int camID)
    {
        saveRecording[camID] = true;
    }

    // Deactivates saving camera replayss for a given camera
    public void DectivateSaveRecording(int camID)
    {
        saveRecording[camID] = false;
    }

    // Returns a bool if a camera is set to save recording
    public bool IsSaved(int camID)
    {
        return saveRecording[camID];
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
            }
        }

        // Check that camID is not a null value
        if(camID != -1)
        {
            // Activate the camera
            ActivateCamera(camID);

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

            // Plays the tripod placement audio clip
            AudioSource audioSource = tripod.GetComponent<AudioSource>();
            audioSource.PlayOneShot(tripodAudio);

            // Set the tilt of the tripod
            // Transform tripodTilt = tripod.transform.Find("PanHandle/PanTransform/TiltHandle/HandleGrip");

            // Set the camera as the target object for the tripod socket interactor
            tripod.GetComponentInChildren<XRSocketInteractor>().StartManualInteraction(cameraObjects[camID].GetComponent<IXRSelectInteractable>());

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
            }
        }

        // Check that camID is not a null value
        if (camID != -1)
        {
            // Activate the camera
            ActivateCamera(camID);

            // Instantiate a tripod at the chosen location
            GameObject tripod = Instantiate(tripodPrefab, location.position, location.rotation);

            // Plays the tripod placement audio clip
            AudioSource audioSource = tripod.GetComponent<AudioSource>();
            audioSource.PlayOneShot(tripodAudio);

            // Set the camera as the target object for the tripod socket interactor
            tripod.GetComponentInChildren<XRSocketInteractor>().StartManualInteraction(cameraObjects[camID].GetComponent<IXRSelectInteractable>());
            
        }
    }
}
