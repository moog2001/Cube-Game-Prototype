using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPos : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject followPositionObject;
    Movement followPositionObjectScript;
    public float rotationSpeed;
    private float horizontal;
    void Start()
    {
        followPositionObjectScript = followPositionObject.GetComponent<Movement>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);
        transform.position = new Vector3(followPositionObjectScript.rayHitPoint.x, transform.position.y, followPositionObjectScript.rayHitPoint.z);
    }
}
