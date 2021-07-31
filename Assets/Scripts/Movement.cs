using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private CinemachineFreeLook freeLookCam;
    // The Rigidbody attached to the GameObject.
    private Rigidbody body;
    /// <summary>
    /// Speed scale for the velocity of the Rigidbody.
    /// </summary>
    public float speed;
    /// <summary>
    /// Rotation Speed scale for turning.
    /// </summary>
    /// <summary>
    /// The upwards jump force of the player.
    /// </summary>
    public float jumpForce;
    public float rotationSpeed;
    // The vertical input from input devices.
    private float vertical;
    // The horizontal input from input devices.

    // Whether or not the player is on the ground.
    private bool isGrounded;
    // Initialization function
    bool freeCam;
    float freeLookCamXValue;
    float freeLookCamYValue;

    public GameObject centerOfMass;
    bool moveTest;
    RaycastHit rayObject;
    Collider collider;
    public Vector3 rayHitPoint;

    private float horizontal;
    RaycastHit rayObjectCache;

    void Start()
    {
        // Obtain the reference to our Rigidbody.
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        freeLookCam = GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>();
        freeLookCamXValue = freeLookCam.m_XAxis.Value;
        freeLookCamYValue = freeLookCam.m_YAxis.Value;
        freeLookCam.m_RecenterToTargetHeading.m_WaitTime = 0.2f;
        freeLookCam.m_YAxisRecentering.m_WaitTime = 0.2f;
        freeLookCam.m_YAxisRecentering.m_RecenteringTime = 0.5f;
        freeLookCam.m_RecenterToTargetHeading.m_RecenteringTime = 0.5f;


    }
    // Fixed Update is called a fix number of frames per second.
    private void Update()
    {
        body.centerOfMass = centerOfMass.transform.localPosition;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            freeCam = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            freeCam = false;
            freeLookCam.m_YAxis.m_InputAxisName = "";
            freeLookCam.m_XAxis.m_InputAxisName = "";
        }
        if (freeCam)
        {
            freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
            freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
        }


        freeLookCam.m_RecenterToTargetHeading.m_enabled = !freeCam;
        freeLookCam.m_YAxisRecentering.m_enabled = !freeCam;


    }


    private void FixedUpdate()
    {

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        //forward moving

        Vector3 velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
        velocity.y = body.velocity.y;
        body.velocity = velocity;

        //turning

        Physics.Raycast(new Vector3(collider.bounds.center.x, collider.bounds.min.y, collider.bounds.center.z), -transform.up, out rayObject, 0.01f);
        if (rayObject.collider != null)
        {
            rayObjectCache = rayObject;
        }
        transform.Rotate((rayObjectCache.normal * horizontal) * rotationSpeed * Time.fixedDeltaTime);

        //jump

        if (Input.GetAxis("Jump") > 0 && IsGrounded())
        {
            body.AddForce(transform.up * jumpForce);
        }
    }
    bool IsGrounded()
    {
        return Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y - 0.1f, collider.bounds.center.z), 0.18f);
    }
}