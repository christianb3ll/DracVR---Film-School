using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tracks whether a usser has achieved a certain shot and updates UI
public class GoalManager : MonoBehaviour
{
    private bool wideAchieved;
    private bool midAchieved;
    private bool mcuAchieved;
    private bool cuAchieved;
    private bool twoshotAchieved;

    public GameObject wideTick;
    public GameObject midTick;
    public GameObject mcuTick;
    public GameObject cuTick;
    public GameObject twoshotTick;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Initialise goals as unachieved
        wideAchieved = false;
        midAchieved = false;
        mcuAchieved = false;
        cuAchieved = false;
        twoshotAchieved = false;

        // Get the success audio source
        audioSource = GetComponent<AudioSource>();
    }

    // Sets a goal as achievved
    public void GoalAchieved(ShotType shot)
    {
        switch (shot)
        {
            case ShotType.WideShot:
                // Wide shot Achieved
                if (!wideAchieved)
                {
                    audioSource.Play();
                    wideAchieved = true;
                    wideTick.SetActive(true);
                }
                
                break;
            case ShotType.TwoShot:
                // Two-Shot Achieved
                if (!twoshotAchieved)
                {
                    audioSource.Play();
                    twoshotAchieved = true;
                    twoshotTick.SetActive(true);
                }
                break;
            case ShotType.MidShot:
                // Mid-Shot Achieved
                if (!midAchieved)
                {
                    audioSource.Play();
                    midAchieved = true;
                    midTick.SetActive(true);
                }
                break;
            case ShotType.MedCloseUp:
                // MCU Achieved
                if (!mcuAchieved)
                {
                    audioSource.Play();
                    mcuAchieved = true;
                    mcuTick.SetActive(true);
                }
                break;
            case ShotType.CloseUp:
                // CU Achieved
                if (!cuAchieved)
                {
                    audioSource.Play();
                    cuAchieved = true;
                    cuTick.SetActive(true);
                }
                break;
        }
        
    }
}
