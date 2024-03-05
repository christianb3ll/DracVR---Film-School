using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages the replay record state of the camera
public class RecordControls : MonoBehaviour
{
    private bool savePresssed;

    public float pressDistance;
    public Material activeMaterial;
    public Material inactiveMaterial;

    public UnityEvent ToggleOn;
    public UnityEvent ToggleOff;

    // Start is called before the first frame update
    void Start()
    {
        // initialise the press state
        savePresssed = false;
    }

    // Toggle the button appearance and set state
    public void ButtonToggle(){
        // Check if button pressed
        if (!savePresssed)
        {
            // Set the button position and material
            gameObject.transform.SetLocalPositionAndRotation(new Vector3(0, pressDistance,0), Quaternion.identity);
            gameObject.GetComponent<MeshRenderer>().material = activeMaterial;
            ToggleOn.Invoke();
        }
        else
        {
            // Set the button position and material
            gameObject.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            gameObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
            ToggleOff.Invoke();
        }

        // Update the press state
        savePresssed = !savePresssed;
    }


}
