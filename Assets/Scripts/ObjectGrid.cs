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
    Mesh playerMesh;
    public enum Orientations
    {
        Right, Left, Top, Forward, Back
    }
    public Orientations orientation;
    public int weight;
    public int height;
    Vector3[] totalVertices;

    Vector3[,] vertices = new Vector3[6, 4];
    private GameObject[] objects;

    void Start()
    {

        player = GameObject.Find("PlayerVisible");
        playerMesh = player.GetComponent<MeshFilter>().mesh;
        playerCollider = player.GetComponent<Collider>();

        totalVertices = playerMesh.vertices;

        objects = new GameObject[totalVertices.GetLength(0)];

        for (int i = 0, k = 0; i < vertices.GetLength(0); i++)
        {
            for (int j = 0; j < vertices.GetLength(1); j++, k++)
            {
                vertices[i, j] = player.transform.InverseTransformPoint(totalVertices[k]);
                objects[k] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                objects[k].transform.position = vertices[i, j];
            }
        }

        gridCreation();

    }



    // Update is called once per frame
    void Update()
    {

    }


    void gridCreation()
    {

    }
    void checkOrientation(Orientations orientation)
    {
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
    }
    void snapToGrid()
    {
        gameObject.transform.SetParent(player.transform, false);
        gameObject.transform.localPosition = spawnPoint;
    }

}
