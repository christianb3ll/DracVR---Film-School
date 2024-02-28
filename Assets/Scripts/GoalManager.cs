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

    // Start is called before the first frame update
    void Start()
    {
        // Initialise goals as unachieved
        wideAchieved = false;
        midAchieved = false;
        mcuAchieved = false;
        cuAchieved = false;
        twoshotAchieved = false;
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
                    wideAchieved = true;
                    wideTick.SetActive(true);
                }
                
                break;
            case ShotType.TwoShot:
                // Two-Shot Achieved
                if (!twoshotAchieved)
                {
                    twoshotAchieved = true;
                    twoshotTick.SetActive(true);
                }
                break;
            case ShotType.MidShot:
                // Mid-Shot Achieved
                if (!midAchieved)
                {
                    midAchieved = true;
                    midTick.SetActive(true);
                }
                break;
            case ShotType.MedCloseUp:
                // MCU Achieved
                if (!mcuAchieved)
                {
                    mcuAchieved = true;
                    mcuTick.SetActive(true);
                }
                break;
            case ShotType.CloseUp:
                // CU Achieved
                if (!cuAchieved)
                {
                    cuAchieved = true;
                    cuTick.SetActive(true);
                }
                break;
        }
        
    }
}
