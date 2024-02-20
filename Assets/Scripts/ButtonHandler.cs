using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonHandler : MonoBehaviour
{
    private GameObject[] camBtns;

    private bool isPressed;
    private Vector3 initialPos;

    public bool toggleableBtn;
    public float pressDistance;

    [SerializeField]
    private Material btnActiveMaterial;
    [SerializeField]
    private Material btnInactiveMaterial;

    public UnityEvent ButtonPress;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        initialPos = gameObject.transform.position;

        if(camBtns == null)
        {
            camBtns = GameObject.FindGameObjectsWithTag("camBtn");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPress()
    {
        if (!isPressed)
        {
            gameObject.transform.position = new Vector3(
            initialPos.x,
            initialPos.y - pressDistance,
            initialPos.z
            );
            ButtonPress.Invoke();
            if (!toggleableBtn)
            {
                StartCoroutine("ButtonReset");
            }
        } else
        {
            gameObject.transform.position = initialPos;
        }

        isPressed = !isPressed;
    }

    public void SetCamBtnMaterials()
    {
        if (isPressed)
        {
            gameObject.GetComponent<MeshRenderer>().material = btnActiveMaterial;
        } else
        {
            gameObject.GetComponent<MeshRenderer>().material = btnInactiveMaterial;
        }
    }

    private bool GetButtonState()
    {
        return isPressed;
    }

    public void SetCamBtnStates()
    {
        foreach (GameObject camBtn in camBtns)
        {
            if(!ReferenceEquals(camBtn, gameObject))
            {
                ButtonHandler btn = camBtn.GetComponent<ButtonHandler>();
                if (btn.GetButtonState())
                {
                    btn.OnPress();
                    btn.SetCamBtnMaterials();
                }
            } 
        }
    }

    IEnumerator ButtonReset()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.position = initialPos;

    }
}
