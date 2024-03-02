using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the display of Shot Hints
public class HintManager : MonoBehaviour
{
    public GameObject hintCanvas;

    // Activates a given shot hint
    public void ActivateHint(GameObject shot)
    {
        hintCanvas.SetActive(true);
        shot.SetActive(true);
    }

    // Deactivates a given shsot hint
    public void DeactivateHint(GameObject shot)
    {
        hintCanvas.gameObject.SetActive(false);
        shot.SetActive(false);
    }
}
