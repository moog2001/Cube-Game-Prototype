using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject player;
    Mesh playerMesh;

    Collider playerCollider;

    Vector3[,] sideVertices = new Vector3[12, 2];
    public int gridNum;
    Vector3[][,] totalCenters;
    private GameObject[] objects = new GameObject[8];


    void Start()
    {
        GetPlayerInfo("CubeTest");

        Vector3[] meshVertices;
        meshVertices = GetVertices(playerMesh);
        meshVertices = meshVertices.Distinct().ToArray();



        Vector3[,] lineVectices;

        lineVectices = cubeVerticesToLineVertices(meshVertices);

        Vector3[][,] totalCenters = new Vector3[3][,];


        for (int i = 0, k = 0; i < totalCenters.GetLength(0); i++)
        {
            for (int j = 0; j < lineVectices.GetLength(0) / totalCenters.GetLength(0); j++, k++)
            {
                for (int n = 0; n < lineVectices.GetLength(1); n++)
                {
                    totalCenters[i][j, n] = lineVectices[k, n];
                }
            }

        }
        for (int i = 0; i < totalCenters.GetLength(0); i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int n = 0; n < 2; n++)
                {
                    print(totalCenters[i][j, n]);
                }
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
    }

    void GetPlayerInfo(string objectName)
    // requires class private GameObject, Mesh and collider to be set up before calling this method.
    {
        player = GameObject.Find(objectName);
        playerMesh = player.GetComponent<MeshFilter>().mesh;
        playerCollider = player.GetComponent<Collider>();
    }
    Vector3[] GetVertices(Mesh mesh)
    {
        Vector3[] totalVertices = playerMesh.vertices;
        return totalVertices;
    }
    Vector3[,] OneToTwoDimentionVecArray(int firstDimention, int secondDimention, Vector3[] oneDimentionArray)
    {
        Vector3[,] twoDimentionVecArray = new Vector3[firstDimention, secondDimention];
        for (int i = 0, k = 0; i < twoDimentionVecArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimentionVecArray.GetLength(1); j++, k++)
            {
                twoDimentionVecArray[i, j] = oneDimentionArray[k];
            }
        }
        return twoDimentionVecArray;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < totalCenters.GetLength(0); i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int n = 0; n < 2; n++)
                {
                    Gizmos.DrawSphere(totalCenters[i][j, n], 0.05f);
                }
            }

        }

    }
    public Vector3[] VectorDivide(Vector3 start, Vector3 end, int divisionNumber)
    {
        Vector3 direction = (end - start).normalized;
        float subVecAmount = Vector3.Distance(start, end) / divisionNumber;

        Vector3[] centers = new Vector3[gridNum];

        for (int i = 0; i < gridNum; i++)
        {
            centers[i] = direction * subVecAmount * (i * 2 + 1) + start;
        }
        return centers;
    }
    Vector3[,] cubeVerticesToLineVertices(Vector3[] cubeVertices)
    {
        // looks very spaghetti but i can't find a pattern to create the line vertices of a cube. its fully compatiple with ProBuilder cubes tho
        Vector3[,] lineVerticesOut = new Vector3[12, 2];

        lineVerticesOut[0, 0] = cubeVertices[0]; lineVerticesOut[0, 1] = cubeVertices[2];
        lineVerticesOut[1, 0] = cubeVertices[1]; lineVerticesOut[1, 1] = cubeVertices[3];
        lineVerticesOut[2, 0] = cubeVertices[4]; lineVerticesOut[2, 1] = cubeVertices[5];
        lineVerticesOut[3, 0] = cubeVertices[6]; lineVerticesOut[3, 1] = cubeVertices[7];

        lineVerticesOut[4, 0] = cubeVertices[0]; lineVerticesOut[4, 1] = cubeVertices[1];
        lineVerticesOut[5, 0] = cubeVertices[2]; lineVerticesOut[5, 1] = cubeVertices[3];
        lineVerticesOut[6, 0] = cubeVertices[6]; lineVerticesOut[6, 1] = cubeVertices[4];
        lineVerticesOut[7, 0] = cubeVertices[7]; lineVerticesOut[7, 1] = cubeVertices[5];

        lineVerticesOut[8, 0] = cubeVertices[0]; lineVerticesOut[8, 1] = cubeVertices[6];
        lineVerticesOut[9, 0] = cubeVertices[1]; lineVerticesOut[9, 1] = cubeVertices[4];
        lineVerticesOut[10, 0] = cubeVertices[3]; lineVerticesOut[10, 1] = cubeVertices[5];
        lineVerticesOut[11, 0] = cubeVertices[2]; lineVerticesOut[11, 1] = cubeVertices[7];

        return lineVerticesOut;
    }
}
