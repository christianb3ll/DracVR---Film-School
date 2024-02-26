using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles methods relating to in-game tablet
public class Tablet : MonoBehaviour
{
    public Animator animator;
    public GameObject tablet;

    public bool hintsActive;
    public GameObject[] hintCanvases;
    public TextMeshProUGUI hintMenuText;

    // Activate the menu on gaze interaction
    public void ActivateMenu()
    {
        tablet.SetActive(true);
        animator.SetTrigger("FlyIn");
    }

    // Deactivate the menu on gaze left
    public void DeactivateMenu()
    {
        animator.SetTrigger("FlyOut");
        HideTablet();
    }

    // Delays deactivating the gameobject until after animation
    IEnumerator HideTablet()
    {
        yield return new WaitForSeconds(0.5f);
        tablet.SetActive(false);
    }

    // Shows/Hides in-game hint screens and upddates tablet menu text
    public void ToggleHints()
    {
        hintsActive = !hintsActive;

        foreach (GameObject canvas in hintCanvases)
        {
            canvas.SetActive(hintsActive);
        }

        if (!hintsActive)
        {
            hintMenuText.text = "Show Hints";
        } else
        {
            hintMenuText.text = "Hide Hints";
        }
        
    }
}
