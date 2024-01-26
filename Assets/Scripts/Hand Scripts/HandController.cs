using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Hand controlling script based on How to ANIMATE Hands in VR - Unity XR Beginner Tutorial (New Input System)
// Justin P Barnett
// https://youtu.be/DxKWq7z4Xao
// Accessed 17/01/2024

[RequireComponent(typeof(ActionBasedController))]
public class HandContoller : MonoBehaviour
{
    ActionBasedController controller;
   
    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}
