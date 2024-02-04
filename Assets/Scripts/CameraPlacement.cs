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
    void Awake()
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


            //Vector3 playerPos = poseManager.GetPlayerPos();
            //Vector3 playerDir = poseManager.GetPlayerDir();
            //Quaternion playerRotation = poseManager.GetPlayerRotation();
            //Quaternion cameraRotation = poseManager.GetPlayerRotation();

            //// Set the camera position to be slightly in front of the player
            //Vector3 newCamPos = playerPos + playerDir * camPositionOffset;

            //GameObject camBody = camera.transform.Find("CamAnchor").gameObject;

            //playerRotation.SetLookRotation(new Vector3(playerDir.x, 0, playerDir.z), Vector3.up);
            //cameraRotation.SetLookRotation(playerDir, Vector3.up);

            //camera.transform.SetPositionAndRotation(new Vector3(newCamPos.x, camHeight + groundLevel, newCamPos.z), playerRotation);
            //camBody.transform.rotation = cameraRotation;

            // initialise position
            Vector3 userPos = Camera.main.transform.position;
            Vector3 userDir = Camera.main.transform.forward;

            // initialise rotation
            Quaternion userRotation = Camera.main.transform.rotation;
            Quaternion cameraRotation = Camera.main.transform.rotation;

            // new possition with camera offset
            Vector3 newPos = userPos + userDir * cameraOffset;

            // Get the tripod head

            // calculate new rotations
            userRotation.SetLookRotation(new Vector3(userDir.x, 0, userDir.z), Vector3.up);
            cameraRotation.SetLookRotation(userDir, Vector3.up);

            // Apply tripod rotation


            // Apply camera rotation


            // Instantiate a tripod at the user's location
            GameObject tripod = Instantiate(
                tripodPrefab, new Vector3(newPos.x, 0, newPos.z) , userRotation);

            // Set the tilt of the tripod
            // Transform tripodTilt = tripod.transform.Find("PanHandle/PanTransform/TiltHandle/HandleGrip");

            // Set the camera as the target object for the tripod socket interactor
            tripod.GetComponent<XRSocketInteractor>().StartManualInteraction(cameraObjects[camID].GetComponent<IXRSelectInteractable>());

        }
    }
}
