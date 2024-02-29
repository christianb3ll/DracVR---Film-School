using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles flickering for title screen projector
public class ProjectorFlicker : MonoBehaviour
{
    public float flickerInterval;
    public float dimIntensity;

    private bool dimmed;
    private float initIntensity;
    private Light lamp;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        lamp = GetComponent<Light>();
        initIntensity = lamp.intensity;
        dimmed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // increment the timer
        timer += Time.deltaTime;

        // Check if timer has passed flicker interval length
        if(timer > flickerInterval)
        {
            // if not dimmed, dim the light
            if (!dimmed)
            {
                lamp.intensity = dimIntensity;
            } else
            {
                // reset intensity
                lamp.intensity = initIntensity;
            }
            // toggle dim state
            dimmed = !dimmed;
            // reset timer
            timer = 0;
        }
    }
}
