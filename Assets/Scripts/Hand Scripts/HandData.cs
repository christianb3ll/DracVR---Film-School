using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hand Pose scripts based on Custom Grab Hand Pose with Unity XR Toolkit
// Valem Tutorials
// https://youtu.be/JdspLj4fZlI?si=ER4EavFoUC78inCP
// Accessed 21/01/2024

public class HandData : MonoBehaviour
{
    public enum HandModelType { left, right}

    public HandModelType HandType;

    public Transform root;
    public Animator animator;
    public Transform[] fingerJoints;

}
