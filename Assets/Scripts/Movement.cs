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
    [SerializeField]  float jumpForce;
    [SerializeField]  float rotationSpeed;
    // The vertical input from input devices.
    private float vertical;
    // The horizontal input from input devices.

    // Whether or not the player is on the ground.
    // Initialization function
    bool freeCam;
    float freeLookCamXValue;
    float freeLookCamYValue;

    public GameObject centerOfMass;
    bool moveTest;
    RaycastHit rayObject;
    Collider objectCollider;
    private float horizontal;
    RaycastHit rayObjectCache;
    float verticalNormalized;
    bool isSprinting;
    [SerializeField] LayerMask mask;

    void Start()
    {
        // Obtain the reference to our Rigidbody.
        body = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        freeLookCam = GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>();
        freeLookCamXValue = freeLookCam.m_XAxis.Value;
        freeLookCamYValue = freeLookCam.m_YAxis.Value;
        freeLookCam.m_RecenterToTargetHeading.m_WaitTime = 0.1f;
        freeLookCam.m_YAxisRecentering.m_WaitTime = 0.1f;
        freeLookCam.m_YAxisRecentering.m_RecenteringTime = 0.1f;
        freeLookCam.m_RecenterToTargetHeading.m_RecenteringTime = 0.1f;
        body.centerOfMass = centerOfMass.transform.localPosition;

    }
    // Fixed Update is called a fix number of frames per second.
    private void Update()
    {


        if (Input.GetMouseButtonDown(2))
        {
            freeCam = true;
        }
        else if (Input.GetMouseButtonUp(2))
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        if (vertical < 0)
        {
            verticalNormalized = -1f;
        }
        else
        {
            verticalNormalized = 1f;
        }


    }


    private void FixedUpdate()
    {


        //forward moving

        if (IsGrounded() && !isSprinting)
        {
            Vector3 velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }
        else if (IsGrounded() && isSprinting)
        {
            Vector3 velocity = (transform.forward * vertical) * speed * 2 * Time.fixedDeltaTime;
            velocity.y = body.velocity.y;
            body.velocity = velocity;
        }

        //turning

        Physics.Raycast(new Vector3(objectCollider.bounds.center.x, objectCollider.bounds.min.y, objectCollider.bounds.center.z), -transform.up, out rayObject, 0.2f, mask.value);
        if (rayObject.collider != null)
        {
            rayObjectCache = rayObject;
        }

        if (rayObjectCache.point != rayObject.point && rayObject.point == null)
        {
            transform.Rotate((transform.up * horizontal) * rotationSpeed * verticalNormalized * Time.fixedDeltaTime);
        }
        else
        {
            transform.Rotate((rayObjectCache.normal * horizontal) * verticalNormalized * rotationSpeed * Time.fixedDeltaTime);
        }

        //jump

        if (Input.GetAxis("Jump") > 0 && IsGrounded())
        {
            body.AddForce(transform.up * jumpForce);
        }

    }
    bool IsGrounded()
    {
        return Physics.CheckCapsule(objectCollider.bounds.center, new Vector3(objectCollider.bounds.center.x, objectCollider.bounds.min.y - 0.05f, objectCollider.bounds.center.z), 0.18f, mask.value);
    }
}
