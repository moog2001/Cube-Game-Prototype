using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGridCreation : MonoBehaviour
{
    GameObject player;
    Mesh playerMesh;


    Collider playerCollider;
    Vector3[,][] centers = new Vector3[3, 4][];
    Vector3[,,] lineVerticesAligned = new Vector3[3, 4, 2];
    [SerializeField] private int gridNumX;
    [SerializeField] private int gridNumY;

    [SerializeField] private int gridNumZ;
    [SerializeField] GameObject scanPrefab;
    [SerializeField] float scanTime;
    private delegate void TestDelegate(); // This defines what type of method you're going to call.
    private TestDelegate methodToCall;


    List<List<Vector3>> scanCenters = new List<List<Vector3>>();

    public bool isDone = false;
    [SerializeField] GameObject rootMovementObject;
    GameObject[] lines;

    void Start()
    {
        //getting player info and components
        GetPlayerInfo(gameObject.name);


        //getting the mesh vertices and removing multiple ones
        Vector3[] meshVertices;
        meshVertices = GetVertices(playerMesh);
        meshVertices = meshVertices.Distinct().ToArray();


        //converting the mesh vertices into line vertices and seperating them to three parts for each dimensions
        Vector3[,] lineVectices;
        lineVectices = cubeVerticesToLineVertices(meshVertices);
        lineVerticesAligned = twoToThreeDimensionVecArray(lineVerticesAligned.GetLength(0), lineVerticesAligned.GetLength(1), lineVerticesAligned.GetLength(2), lineVectices);



        //setting up the grid centers on the lines based on the grid num and line lengths
        for (int j = 0; j < centers.GetLength(1); j++)
        {
            centers[0, j] = VectorDivide(lineVerticesAligned[0, j, 0], lineVerticesAligned[0, j, 1], gridNumX);
        }
        for (int j = 0; j < centers.GetLength(1); j++)
        {
            centers[1, j] = VectorDivide(lineVerticesAligned[1, j, 0], lineVerticesAligned[1, j, 1], gridNumY);
        }
        for (int j = 0; j < centers.GetLength(1); j++)
        {
            centers[2, j] = VectorDivide(lineVerticesAligned[2, j, 0], lineVerticesAligned[2, j, 1], gridNumZ);
        }




        //creating line meshes for grid line centers and giving them their appropiate names

        lines = new GameObject[gridNumX];

        int k = 0;
        for (int j = 0; j < centers[0, 0].GetLength(0); j++, k++)
        {
            lines[k] = LineRender(centers[0, 0][j], centers[0, 1][j], centers[0, 3][j], centers[0, 2][j], "x", gameObject);
        }
        /*
        for (int j = 0; j < centers[1, 0].GetLength(0); j++, k++)
        {
            lines[k] = LineRender(centers[1, 0][j], centers[1, 1][j], centers[1, 3][j], centers[1, 2][j], "y", gameObject);
        }
        for (int j = 0; j < centers[2, 0].GetLength(0); j++, k++)
        {
            lines[k] = LineRender(centers[2, 0][j], centers[2, 1][j], centers[2, 3][j], centers[2, 2][j], "z", gameObject);
        }
        */

        methodToCall = Start2;
        StartCoroutine(waitForScanSingle(1.5f));

    }


    void Start2()
    {
        for (int i = 0; i < centers[1, 0].GetLength(0); i++)
        {
            Vector3[] travelPoints = new Vector3[] { centers[1, 0][i], centers[1, 1][i], centers[1, 3][i], centers[1, 2][i], centers[1, 0][i] };
            ScanLines(travelPoints, scanPrefab, "x");
        }
        methodToCall = Start3;
        StartCoroutine(waitForScanSingle(scanTime * 4 + 1f));

    }
    void Start3()
    {
        foreach(GameObject x in lines){
            Destroy(x);
        }
        isDone = true;
        foreach (List<Vector3> x in scanCenters)
        {
            print(x.Count);
        }
        Rigidbody rootMovementRb = rootMovementObject.GetComponent<Rigidbody>();
        rootMovementRb.isKinematic = false;        
    }


    // Update is called once per frame


    void GetPlayerInfo(string objectName)
    // requires class private GameObject, Mesh and collider to be set up before calling this method.
    {
        player = GameObject.Find(objectName);
        playerMesh = player.GetComponent<MeshFilter>().mesh;
        playerCollider = player.GetComponent<Collider>();
        GetComponents<LineRenderer>();
    }
    Vector3[] GetVertices(Mesh mesh)
    {
        Vector3[] totalVertices = new Vector3[playerMesh.vertices.Length];
        for (int i = 0; i < totalVertices.Length; i++)
        {
            totalVertices[i] = transform.TransformPoint(playerMesh.vertices[i]);
        }
        return totalVertices;
    }
    Vector3[,] OneToTwoDimensionVecArray(int firstDimension, int secondDimension, Vector3[] oneDimensionArray)
    {
        Vector3[,] twoDimensionVecArray = new Vector3[firstDimension, secondDimension];
        for (int i = 0, k = 0; i < twoDimensionVecArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimensionVecArray.GetLength(1); j++, k++)
            {
                twoDimensionVecArray[i, j] = oneDimensionArray[k];
            }
        }
        return twoDimensionVecArray;
    }
    Vector3[,,] twoToThreeDimensionVecArray(int firstDimension, int secondDimension, int thirdDimension, Vector3[,] twoDimensionArray)
    {
        Vector3[,,] threeDimensionArray = new Vector3[firstDimension, secondDimension, thirdDimension];

        for (int i = 0, k = 0; i < firstDimension; i++)
        {
            for (int j = 0; j < secondDimension; j++, k++)
            {
                for (int n = 0; n < thirdDimension; n++)
                {
                    threeDimensionArray[i, j, n] = twoDimensionArray[k, n];
                }
            }
        }
        return threeDimensionArray;
    }
    /*
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            for (int i = 0; i < lineVerticesAligned.GetLength(0); i++)
            {
                for (int j = 0; j < lineVerticesAligned.GetLength(1); j++)
                {
                    for (int n = 0; n < lineVerticesAligned.GetLength(2); n++)
                    {
                        Gizmos.DrawSphere(lineVerticesAligned[i, j, n], 0.05f);
                    }
                }
            }
            Gizmos.color = Color.red;




            for (int i = 0; i < centers.GetLength(0); i++)
            {
                for (int j = 0; j < centers.GetLength(1); j++)
                {
                    for (int n = 0; n < centers[i, j].GetLength(0); n++)
                    {
                        Gizmos.DrawSphere(centers[i, j][n], 0.05f);
                    }
                }
            }


        }
    */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        foreach (List<Vector3> x in scanCenters)
        {
            foreach (Vector3 y in x)
            {
                Gizmos.DrawSphere(y, 0.5f);
            }

        }

    }
    public Vector3[] VectorDivide(Vector3 start, Vector3 end, int gridNum)
    {
        float divisionNumber = gridNum * 2;
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
        //x
        lineVerticesOut[0, 0] = cubeVertices[1]; lineVerticesOut[0, 1] = cubeVertices[0];
        lineVerticesOut[1, 0] = cubeVertices[3]; lineVerticesOut[1, 1] = cubeVertices[2];
        lineVerticesOut[2, 0] = cubeVertices[4]; lineVerticesOut[2, 1] = cubeVertices[6];
        lineVerticesOut[3, 0] = cubeVertices[5]; lineVerticesOut[3, 1] = cubeVertices[7];
        //y
        lineVerticesOut[4, 0] = cubeVertices[1]; lineVerticesOut[4, 1] = cubeVertices[3];
        lineVerticesOut[5, 0] = cubeVertices[0]; lineVerticesOut[5, 1] = cubeVertices[2];
        lineVerticesOut[6, 0] = cubeVertices[4]; lineVerticesOut[6, 1] = cubeVertices[5];
        lineVerticesOut[7, 0] = cubeVertices[6]; lineVerticesOut[7, 1] = cubeVertices[7];
        //z
        lineVerticesOut[8, 0] = cubeVertices[1]; lineVerticesOut[8, 1] = cubeVertices[4];
        lineVerticesOut[9, 0] = cubeVertices[3]; lineVerticesOut[9, 1] = cubeVertices[5];
        lineVerticesOut[10, 0] = cubeVertices[0]; lineVerticesOut[10, 1] = cubeVertices[6];
        lineVerticesOut[11, 0] = cubeVertices[2]; lineVerticesOut[11, 1] = cubeVertices[7];

        return lineVerticesOut;
    }
    GameObject LineRender(Vector3 one, Vector3 two, Vector3 three, Vector3 four, string name, GameObject parent)
    {
        GameObject lineObject = new GameObject();
        lineObject.name = name;
        lineObject.transform.parent = gameObject.transform;


        MeshCollider meshCollider = lineObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        Rigidbody rigidbody = lineObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;



        LineRenderer Line = lineObject.AddComponent<LineRenderer>();
        Line.positionCount = 5;
        Line.widthMultiplier = 0.01f;

        Vector3[] points = new Vector3[] { one, two, three, four, one };
        Line.SetPositions(points);

        Mesh outLineMesh = new Mesh();
        Line.BakeMesh(outLineMesh, true);
        meshCollider.sharedMesh = outLineMesh;
        return lineObject;
    }

    void ScanLines(Vector3[] travelPoints, GameObject prefab, string collisionObjectName)
    {
        GameObject scanCube = Instantiate(prefab, travelPoints[0], prefab.transform.rotation);
        scanCube.transform.parent = gameObject.transform;
        Rigidbody objectRigidBody = scanCube.AddComponent<Rigidbody>();
        objectRigidBody.useGravity = false;
        objectRigidBody.isKinematic = true;
        FollowPositions followPositions = scanCube.GetComponent<FollowPositions>();
        followPositions.parentObject = gameObject;
        followPositions.collisionObjectName = collisionObjectName;
        followPositions.StartCoroutine(scanCube.GetComponent<FollowPositions>().FollowVector3Points(scanTime, travelPoints));
        StartCoroutine(waitForScan(scanTime * 4 + 1f, followPositions));
    }
    IEnumerator waitForScan(float scanWaitTime, FollowPositions scanObjectFollowPositions)
    {

        yield return new WaitForSeconds(scanWaitTime);


        scanCenters.Add(scanObjectFollowPositions.hitPoints);

    }
    IEnumerator waitForScanSingle(float scanWaitTime)
    {

        yield return new WaitForSeconds(scanWaitTime);
        SimpleMethod();

    }
    private void SimpleMethod()
    {
        methodToCall();
    }


}