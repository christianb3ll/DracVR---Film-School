using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Methods relating to the in-game console
public class ConsoleManager : MonoBehaviour
{
    private GameObject[] camBtns;

    public MeshRenderer[] camScreens;
    public Material noSignalMaterial;
    public Material[] camMaterials;

    // Start is called before the first frame update
    void Start()
    {
        // Get all cam buttons
        if (camBtns == null)
        {
            camBtns = GameObject.FindGameObjectsWithTag("camBtn");
        }
    }

    // Activates the camera screen on the console
    public void ActivateCamScreen(int camID)
    {
        camScreens[camID].material = camMaterials[camID];
    }

    // Sets the no signal screen on the console
    public void DeactivateCamScreen(int camID)
    {
        camScreens[camID].material = noSignalMaterial;
    }

    // Takes a given camera button and deactivates all other buttons
    public void SetBtnStates(GameObject button)
    {
        foreach (GameObject camBtn in camBtns)
        {
            if (!ReferenceEquals(camBtn, button))
            {
                ButtonHandler btn = camBtn.GetComponent<ButtonHandler>();
                if (btn.ButtonIsPressed())
                {
                    btn.DeactivateBtn();
                }
            }
        }
    }
}
