using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

// CameraArea class handles the interface for placing cameras
// and adds functionality for manipulating rotation
public class CameraArea : MonoBehaviour
{
    private XRSimpleInteractable interactable;

    public CameraPlacement cameraPlacement;

    // User input variables
    public InputActionReference thumbstickLeft;
    public float rotateSpeed;

    public GameObject reticle;

    void Start()
    {
        // Get the interactable
        interactable = GetComponent<XRSimpleInteractable>();
    }

    void Update()
    {
        // Check if the interactable is currently selected
        if (interactable.isSelected)
        {
            ShowReticle();

            // Get the rotation value of the thumbstick
            float rotation = thumbstickLeft.action.ReadValue<Vector2>().x;

            // Apply the rotation around the y axis
            reticle.transform.Rotate(0, rotation * rotateSpeed, 0);
        }
    }

    // Activates the reticle on hover and updates position
    public void OnHover()
    {
        ShowReticle();
        reticle.transform.position = gameObject.transform.position;
    }

    // Show reticle
    public void ShowReticle()
    {
        reticle.SetActive(true);
    }

    // Hide reticle
    public void HideReticle()
    {
        reticle.SetActive(false);
    }

    // Places a camera at the reticle position
    public void OnActivate()
    {
        cameraPlacement.PlaceCamera(reticle.transform);
    }

}
