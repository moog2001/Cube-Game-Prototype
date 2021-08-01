using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrid : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    Transform objectTransform;
    Collider playerCollider;
    Vector3 spawnPoint;
    float xValue;
    Vector3 testValue;
    public enum Orientations
    {
        Right, Left, Top, Forward, Back
    }
    public Orientations orientation;

    void Start()
    {
        player = GameObject.Find("PlayerVisible");
        playerCollider = player.GetComponent<Collider>();
        /*
                switch (orientation)
                {
                    case Orientations.Forward:
                        {
                            spawnPoint = player.transform.InverseTransformPoint(new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.center.y, playerCollider.bounds.max.z));
                            break;
                        }
                    case Orientations.Back:
                        {

                            spawnPoint = player.transform.InverseTransformPoint(new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.center.y, playerCollider.bounds.min.z));
                            break;
                        }
                    case Orientations.Left:
                        {

                            spawnPoint = player.transform.InverseTransformPoint(new Vector3(playerCollider.bounds.min.x, playerCollider.bounds.center.y, playerCollider.bounds.center.z));
                            break;
                        }

                    case Orientations.Right:
                        {

                            spawnPoint = player.transform.InverseTransformPoint(new Vector3(playerCollider.bounds.max.x, playerCollider.bounds.center.y, playerCollider.bounds.center.z));
                            break;
                        }
                    case Orientations.Top:
                        {

                            spawnPoint = player.transform.InverseTransformPoint(new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.max.y, playerCollider.bounds.center.z));
                            break;
                        }
                }

                gameObject.transform.SetParent(player.transform, false);
                gameObject.transform.localPosition = spawnPoint;
                */
        print(playerCollider.bounds.max.x);
        print(playerCollider.bounds.min.x);
        testValue = new Vector3(playerCollider.bounds.max.x, playerCollider.bounds.min.y, playerCollider.bounds.max.z) - new Vector3(playerCollider.bounds.min.x, playerCollider.bounds.min.y, playerCollider.bounds.min.z);
        print(testValue);
        xValue = testValue.x;
        print(xValue);

    }



    // Update is called once per frame
    void Update()
    {

    }
}
