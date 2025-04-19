using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask WhatIsGrapple;
    public Transform gunTip, Camera, player;
    public float maxDistance;
    private SpringJoint joint;

    private bool isGrappling = false;

    public PlayerMovement pl;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            StopGrapple();
        }
        if(isGrappling)
        {
            pl.moveSpeed = pl.originalSpeed * 1.5f;
        }
        else if(!isGrappling)
        {
            pl.moveSpeed = pl.originalSpeed;
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
     
        RaycastHit hit;
        if(Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance, WhatIsGrapple))
        {
            isGrappling = true;
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 1; //0.8
            joint.minDistance = distanceFromPoint * 0; //0.25

            joint.spring = 65f; //4,5
            joint.damper = 10; //7
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        isGrappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }
}
