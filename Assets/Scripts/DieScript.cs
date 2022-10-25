using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieScript : MonoBehaviour
{
    CharacterController controller;
    Rigidbody rb;
    public TextMeshProUGUI text;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            controller.enabled = false;
            rb.isKinematic = false;  
            rb.AddForce(coll.gameObject.transform.up * 2000f, ForceMode.Force);
            rb.AddForce(coll.gameObject.transform.forward * 2000f, ForceMode.Force);
            text.enabled = true;
        }
    }
}
