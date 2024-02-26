using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Handles in-game buttons and their associated methods
public class ButtonHandler : MonoBehaviour
{
    public ConsoleManager console;

    private bool isPressed;
    private Vector3 initialPos;

    public bool toggleableBtn;
    public bool cameraBtn;
    public float pressDistance;

    [SerializeField]
    private Material btnActiveMaterial;
    [SerializeField]
    private Material btnInactiveMaterial;

    public UnityEvent ButtonPress;

    // Start is called before the first frame update
    void Start()
    {
        // initialise button to unpressed state
        isPressed = false;
        initialPos = gameObject.transform.position;
    }

    // Called when the user selects a button
    public void OnPress()
    {
        // temporary button state bool
        bool currentlyPressed = isPressed;

        // Check button is not currently pressed
        if(!currentlyPressed)
        {
            // Activate the button
            ActivateBtn();

            // Check if the button is toggleable or not
            if (!toggleableBtn)
            {
                // Resets the button position after a delay
                StartCoroutine("ButtonReset");
            }
        }
        else
        {
            // Deactivates the button
            // Camera buttons can only be deactivated by selecting a
            // different camera button
            if(!cameraBtn) DeactivateBtn();
        }
    }

    // Returns the current button presss state
    public bool ButtonIsPressed()
    {
        return isPressed;
    }

    // Activates the button
    public void ActivateBtn()
    {
        // Sets the position of the button according to press distance
        gameObject.transform.position = new Vector3(
            initialPos.x,
            initialPos.y - pressDistance,
            initialPos.z
            );
        // Invokes the events associated with the button
        ButtonPress.Invoke();

        // Check if button is a camera button
        if (cameraBtn)
        {
            // Set the button material to active material
            gameObject.GetComponent<MeshRenderer>().material = btnActiveMaterial;
            // Deactivates the other camera buttons
            console.SetBtnStates(gameObject);
        }
        // Set pressed state to active
        isPressed = true;
    }

    // Deactivates the button
    public void DeactivateBtn()
    {
        // returns the button to the unset position
        gameObject.transform.position = initialPos;
        // Check if button is a camera button
        if (cameraBtn)
        {
            // Set the button material to inactive material
            gameObject.GetComponent<MeshRenderer>().material = btnInactiveMaterial;
        }
        // Set pressed state to inactive
        isPressed = false;
    }

    // resets the button position based on a timer
    IEnumerator ButtonReset()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.position = initialPos;
        isPressed = !isPressed;
    }
}
