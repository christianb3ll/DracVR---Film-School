using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles methods relating to in-game tablet
public class Tablet : MonoBehaviour
{

    public Animator animator;
    public GameObject tablet;

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
}
