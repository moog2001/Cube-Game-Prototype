using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionContacts : MonoBehaviour
{
    // Start is called before the first frame update
    public string colliderName;
    public bool isActive = false;


    public List<Vector3> centerPoints = new List<Vector3>();



    void OnCollisionEnter(Collision other)
    {
        if (isActive)
        {
            foreach (ContactPoint contact in other.contacts)
            {
                print(contact.thisCollider.name + " hit " + contact.otherCollider.name);

                centerPoints.Add(contact.point);
            }

        }
    }
    private void OnDrawGizmos()
    {
        foreach (Vector3 point in centerPoints)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
