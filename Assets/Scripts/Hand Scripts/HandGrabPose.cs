using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

// Hand Pose scripts based on Custom Grab Hand Pose with Unity XR Toolkit
// Valem Tutorials
// https://youtu.be/JdspLj4fZlI?si=ER4EavFoUC78inCP
// Accessed 21/01/2024

public class HandGrabPose : MonoBehaviour
{
    public HandData rightHandPose;
    public HandData leftHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    // Start is called before the first frame update
    void Start()
    {
        // Get the interactable component
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        XRKnob knob = GetComponent<XRKnob>();

        if (grabInteractable != null)
        {
            // Add the event listeners
            grabInteractable.selectEntered.AddListener(SetupPose);
            grabInteractable.selectExited.AddListener(UnsetPose);
            
        }

        if(knob != null)
        {
            // Add the event listeners
            knob.selectEntered.AddListener(SetupPose);
            knob.selectExited.AddListener(UnsetPose);
        }

        // disables the pose object on start by default
        rightHandPose.gameObject.SetActive(false);
        leftHandPose.gameObject.SetActive(false);

    }

    // Sets up the hand pose
    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if(arg.interactorObject is XRDirectInteractor)
        {
            // Get the interactor parent
            GameObject parentHand = arg.interactorObject.transform.parent.gameObject;
            // Get the hand data from the hand object
            HandData handData = parentHand.GetComponentInChildren<HandData>();
            // Disable the animator on the hand
            handData.animator.enabled = false;

            // Determine which hand needs to be posed
            if(handData.HandType == HandData.HandModelType.right)
            {
                SetHandDataValues(handData, rightHandPose);
            }
            else
            {
                SetHandDataValues(handData, leftHandPose);
            }

            // Apply the pose data
            SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
        }
    }

    // Unsets the grab pose
    public void UnsetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            // Get the interactor parent
            GameObject parentHand = arg.interactorObject.transform.parent.gameObject;
            // Get the hand data from the hand object
            HandData handData = parentHand.GetComponentInChildren<HandData>();
            // Disable the animator on the hand
            handData.animator.enabled = true;

            SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
        }
    }

    // Sets the values for hand data
    public void SetHandDataValues(HandData hand1, HandData hand2)
    {
        // Setup the start and end positio of the hand
        startingHandPosition = hand1.root.localPosition;
        finalHandPosition = hand2.root.localPosition;

        // Setup the start and end rotation of the hand
        startingHandRotation = hand1.root.localRotation;
        finalHandRotation = hand2.root.localRotation;

        // Setup the rotation arrays
        startingFingerRotations = new Quaternion[hand1.fingerJoints.Length];
        finalFingerRotations = new Quaternion[hand1.fingerJoints.Length];

        // Iterate over the finger joints to populate the values
        for(int i = 0; i < hand1.fingerJoints.Length; i++)
        {
            startingFingerRotations[i] = hand1.fingerJoints[i].localRotation;
            finalFingerRotations[i] = hand2.fingerJoints[i].localRotation;
        }

    }

    // Applies the new hand data values
    public void SetHandData(HandData hData, Vector3 newPosition, Quaternion newRotation, Quaternion[] newFingerRotations)
    {
        hData.root.localPosition = newPosition;
        hData.root.localRotation = newRotation;

        for (int i = 0; i < newFingerRotations.Length; i++)
        {
            hData.fingerJoints[i].localRotation = newFingerRotations[i];
        }
    }


}
