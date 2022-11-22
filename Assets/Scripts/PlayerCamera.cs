using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    public float minX = -60f;
    public float maxX = 60f;
 
    public float sensitivity;
    public Camera cam;
 
    float rotY = 0f;
    float rotX = 0f;
    
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        if (!HasInputAuthority)
        {
            cam.enabled = false;
            if (cam.gameObject.TryGetComponent(out AudioListener audioListener))
            {
                audioListener.enabled = false;
            }
        }
    }

 
    void Update()
    {
        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX += Input.GetAxis("Mouse Y") * sensitivity;
 
        rotX = Mathf.Clamp(rotX, minX, maxX);
 
        transform.localEulerAngles = new Vector3(0, rotY, 0);
        cam.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
    }

}
