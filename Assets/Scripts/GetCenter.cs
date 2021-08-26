using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetCenter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] exceptionGameobjects;
    public Vector3 getCenter(Transform obj)
    {
        Vector3 center = new Vector3();
        if (obj.GetComponent<Renderer>() != null)
        {
            center = obj.GetComponent<Renderer>().bounds.center;
        }
        else
        {

            foreach (Transform subObj in obj)
            {
                if (!exceptionGameobjects.Contains(subObj.gameObject))
                {
                    center += getCenter(subObj);

                }
            }
            center /= obj.childCount;
        }
        return center;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(getCenter(transform), 0.04f);
    }
}
