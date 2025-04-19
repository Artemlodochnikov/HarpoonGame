using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float originalSpeed;
    public float GravityScale;
    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readytoJump;

    [Header("CoyoteTime")]
    private float coyoteTime = 0.2f;
    private float coyoteTimeOriginal;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    public float Distance;
    public LayerMask WhatIsGround;
    bool grounded;

    [Header("Camera")]
    public Transform orientation;

    float horizontal;
    float vertical;

    Rigidbody rb;
    Vector3 moveDirection;

    void Start()
    {
        coyoteTimeOriginal = coyoteTime;
        originalSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readytoJump = true;
    }

    void Update()
    {
        Debug.Log(rb.linearVelocity.x);
        MyInput();
        speedControl();

        Ray ray = new Ray(transform.position, -Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction * Distance);

        if (Physics.Raycast(ray, Distance, WhatIsGround))
            grounded = true;
        else
            grounded = false;

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readytoJump && (grounded || coyoteTime >= 0))
        {
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        if (grounded)
        {
            coyoteTime = coyoteTimeOriginal;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            coyoteTime -= Time.deltaTime;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            rb.AddForce(-transform.up * GravityScale, ForceMode.Force);
        }
    }
    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

    }
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readytoJump = true;
    }
}