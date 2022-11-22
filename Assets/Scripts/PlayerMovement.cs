using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    CharacterController controller;
    Rigidbody rb;
    public Transform groundCheck;
    public LayerMask groundMask;

    public TextMeshProUGUI text;

    Vector3 move;
    Vector3 input;
    Vector3 Yvelocity;

    public float speed;
    public float airSpeed;
    public float gravity;
    public float jumpHeight;

    bool isGrounded;
    bool isAlive = true;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void HandleInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Update()
    {
        if (!HasInputAuthority) return;
        
        if (isAlive)
        {
            CheckIfGrounded();
            HandleInput();
            if (isGrounded)
            {
                GroundMovement();
            }
            else
            {
                AirMovement();
            }

            controller.Move(move * Time.deltaTime);
            ApplyGravity();
        }
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
    }

    void GroundMovement()
    {
        if (input.x != 0)
        {
            move.x += input.x * speed;
        }
        else
        {
            move.x = 0;
        }

        if (input.z != 0)
        {
            move.z += input.z * speed;
        }
        else
        {
            move.z = 0;
        }

        move = Vector3.ClampMagnitude(move, speed);
    }

    void AirMovement()
    {
        move.z += input.z * airSpeed;
        move.x += input.x * airSpeed;
        move = Vector3.ClampMagnitude(move, speed);
    }


    void ApplyGravity()
    {
        Yvelocity.y += gravity * Time.deltaTime;
        controller.Move(Yvelocity * Time.deltaTime);
    }

    void Jump()
    {
        Yvelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            isAlive = false;
            controller.enabled = false;
            rb.isKinematic = false;
            rb.AddForce(coll.gameObject.transform.forward * 2000f, ForceMode.Force);
            text.enabled = true;
        }
    }
}