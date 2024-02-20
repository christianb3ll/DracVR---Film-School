using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonHandler : MonoBehaviour
{
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
}
