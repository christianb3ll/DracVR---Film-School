using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripodSetup : MonoBehaviour
{
    public Transform headTransform;
    public GameObject tripodHead;

    // Start is called before the first frame update
    void Start()
    {
        GameObject head = Instantiate(tripodHead, headTransform);

        head.transform.parent = headTransform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
