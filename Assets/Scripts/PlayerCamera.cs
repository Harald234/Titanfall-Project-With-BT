using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float minX = -60f;
    public float maxX = 60f;
 
    public float sensitivity;
    public Camera cam;
 
    float rotY = 0f;
    float rotX = 0f;
 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
