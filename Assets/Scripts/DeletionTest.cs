using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionTest : MonoBehaviour
{
    private bool tripodHeld;
    public Transform attachHeight;

    // Start is called before the first frame update
    void Start()
    {
        tripodHeld = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tripodHeld)
        {
            if(attachHeight.position.y > Camera.main.transform.position.y)
            {
                Destroy(gameObject);
                tripodHeld = false;
            }
        }
    }

    public void SetHeld()
    {
        tripodHeld = true;
    }

    public void SetReleased()
    {
        tripodHeld = false;
    }


}
