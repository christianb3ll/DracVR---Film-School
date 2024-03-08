using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UltimateReplay;
using UltimateReplay.Storage;

// Managed the placement of cameras
public class CameraManager : MonoBehaviour
{
    public GameObject[] cameraObjects;
    public GameObject tripodPrefab;

    public AudioClip tripodAudio;

    public ConsoleManager consoleManager;

    public GameObject initCamTransform;

    public float cameraOffset;

    private CameraReplay[] replayObjects;

    // initialises replay objects
    void Awake()
    {
        replayObjects = new CameraReplay[5];
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            replayObjects[i] = new CameraReplay(
                i,
                cameraObjects[i].GetComponent<ReplayObject>(),
                false,
                new ReplayMemoryStorage("CameraStorage" + i)
                );
        }
    }

    // Checks if a camera is active in the scene
    public bool IsActive(int camID)
    {
        return cameraObjects[camID].activeInHierarchy;
    }

    // Method for Activating a camera by ID
    public void ActivateCamera(int camID)
    {
        // Activate the camera
        cameraObjects[camID].SetActive(true);

        // Set the camera screen
        consoleManager.ActivateCamScreen(camID);
    }

    // Method for Deactivating a camera by ID
    public void DeactivateCamera(int camID)
    {
        cameraObjects[camID].SetActive(false);

        // Set the camera screen to no signal
        consoleManager.DeactivateCamScreen(camID);
    }

    // Starts record operations for all active cameras
    public void StartRecording()
    {
        for(int i = 0; i < cameraObjects.Length; i++)
        {
            if (cameraObjects[i].activeInHierarchy)
            {
                replayObjects[i].StartRecording();
            }
        }
    }

    // Ends record operations for all active cameras
    public void EndRecording()
    {
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if (cameraObjects[i].activeInHierarchy)
            {
                replayObjects[i].EndRecording();
            }
        }
    }

    // Starts playback operations for all active cameras
    public void StartPlayback()
    {
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if (cameraObjects[i].activeInHierarchy)
            {
                replayObjects[i].StartPlayback();
            }
        }
    }

    // Ends playback for all active cameras
    public void EndPlayback()
    {
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if (cameraObjects[i].activeInHierarchy)
            {
                replayObjects[i].EndPlayback();
            }
        }
    }

    // Set the save state to true for a given camera
    public void SetSaveActive(int camID)
    {
        replayObjects[camID].saved = true;
    }

    // Set the save state to false for a given camera
    public void SetSaveInactive(int camID)
    {
        replayObjects[camID].saved = false;
    }

    // Method for checking if cameras can be placed
    public bool CamerasAvailable()
    {
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if (cameraObjects[i].activeInHierarchy == false)
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
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if(cameraObjects[i].activeInHierarchy == false && camID == -1)
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
        for (int i = 0; i < cameraObjects.Length; i++)
        {
            if (cameraObjects[i].activeInHierarchy == false && camID == -1)
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

    // Places a tripod at the player location
    public void PlaceTripod()
    {
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
            tripodPrefab, new Vector3(newPos.x, 0, newPos.z), userRotation);

        // Plays the tripod placement audio clip
        AudioSource audioSource = tripod.GetComponent<AudioSource>();
        audioSource.PlayOneShot(tripodAudio);
    }
}
