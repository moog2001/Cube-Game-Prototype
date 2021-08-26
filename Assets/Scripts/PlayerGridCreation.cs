using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using System.Linq;
using UnityEditor;
using UnityEngine;

public struct GridCenterTransforms
    {
        public bool IsFree;
        public Vector3 position;
        public string name;
    }


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
    [SerializeField]GUIStyle style;
    private delegate void TestDelegate(); // This defines what type of method you're going to call.
    private TestDelegate methodToCall;
    public GridCenterTransforms[][,] gridCenterTransforms;
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



        

       
       gridCenterTransforms = new GridCenterTransforms[5][,]
       {
        new GridCenterTransforms[gridNumX,gridNumY],
        new GridCenterTransforms[gridNumX,gridNumY],
        new GridCenterTransforms[gridNumZ,gridNumY],
        new GridCenterTransforms[gridNumZ,gridNumY],
        new GridCenterTransforms[gridNumX, gridNumZ],
       };
        // x and y 
        for (int i = 0; i < gridCenterTransforms[0].GetLength(0); i++)
        {
            for (int j = 0; j < gridCenterTransforms[0].GetLength(1); j++)
            {
                gridCenterTransforms[0][i, j].position = GetIntersection(centers[0, 0][i], centers[0, 1][i], centers[1, 0][j], centers[1, 1][j]);
                gridCenterTransforms[0][i, j].IsFree = true;
                gridCenterTransforms[0][i, j].name = "x+ : " + i.ToString() + ", " + j.ToString();
            }
        }
        //x and y
        for (int i = 0; i < gridCenterTransforms[1].GetLength(0); i++)
        {
            for (int j = 0; j < gridCenterTransforms[1].GetLength(1); j++)
            {
                gridCenterTransforms[1][i, j].position = GetIntersection(centers[0, 2][i], centers[0, 3][i], centers[1, 2][j], centers[1, 3][j]);
                gridCenterTransforms[1][i, j].IsFree = true;
                gridCenterTransforms[1][i, j].name = "x- : " + i.ToString() + ", " + j.ToString();
            }
        }
        // z and y
        for (int i = 0; i < gridCenterTransforms[2].GetLength(0); i++)
        {
            for (int j = 0; j < gridCenterTransforms[2].GetLength(1); j++)
            {
                gridCenterTransforms[2][i, j].position = GetIntersection(centers[2, 0][i], centers[2, 1][i], centers[1, 0][j], centers[1, 2][j]);
                gridCenterTransforms[2][i, j].IsFree = true;
                gridCenterTransforms[2][i, j].name ="z+ : " + i.ToString() + ", " + j.ToString();
            }
        }
        // z and y
        for (int i = 0; i < gridCenterTransforms[3].GetLength(0); i++)
        {
            for (int j = 0; j < gridCenterTransforms[3].GetLength(1); j++)
            {
                gridCenterTransforms[3][i, j].position =GetIntersection(centers[2, 2][i], centers[2, 3][i], centers[1, 3][j], centers[1, 1][j]);
                gridCenterTransforms[3][i, j].IsFree= true;
                gridCenterTransforms[3][i, j].name = "z- : " + i.ToString() + ", " + j.ToString();
            }
        }
        // x and z
        for (int i = 0; i < gridCenterTransforms[4].GetLength(0); i++)
        {
            for (int j = 0; j < gridCenterTransforms[4].GetLength(1); j++)
            {
                gridCenterTransforms[4][i, j].position = GetIntersection(centers[0, 1][i], centers[0, 3][i], centers[2, 1][j], centers[2, 3][j]);
                gridCenterTransforms[4][i, j].IsFree = true;
                gridCenterTransforms[4][i, j].name = "y+ : " +i.ToString() + ", " + j.ToString();
            }
        }
        PlayerBehaviorController playerBehaviorController = GetComponent<PlayerBehaviorController>();        


        // animation for the scanning thing
        for (int i = 0; i < centers.GetLength(0); i++)
        {
            for (int j = 0; j < centers[i, 0].GetLength(0); j++)
            {
                Vector3[] travelPoints = new Vector3[] { centers[i, 0][j], centers[i, 1][j], centers[i, 3][j], centers[i, 2][j], centers[i, 0][j] };
                ScanLines(travelPoints, scanPrefab);
            }
        }


    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        

        for (int i = 0; i < gridCenterTransforms.GetLength(0); i++)
        {
            for (int j = 0; j < gridCenterTransforms[i].GetLength(0); j++)
            {
                for (int n = 0; n < gridCenterTransforms[i].GetLength(1); n++)
                {
                    Gizmos.DrawSphere(transform.TransformPoint(gridCenterTransforms[i][j,n].position), 0.01f);
                    Handles.Label(transform.TransformPoint(gridCenterTransforms[i][j,n].position), gridCenterTransforms[i][j,n].name, style);
                
                }
            }
        }
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
            totalVertices[i] = playerMesh.vertices[i];
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

    void ScanLines(Vector3[] travelPoints, GameObject prefab)
    {
        GameObject scanCube = Instantiate(prefab, travelPoints[0], prefab.transform.rotation);
        Rigidbody objectRigidBody = scanCube.AddComponent<Rigidbody>();
        objectRigidBody.useGravity = false;
        objectRigidBody.isKinematic = true;
        FollowPositions followPositions = scanCube.GetComponent<FollowPositions>();
        followPositions.parentObject = gameObject;
        followPositions.StartCoroutine(scanCube.GetComponent<FollowPositions>().FollowVector3Points(scanTime, travelPoints));
    }

    Vector3 GetIntersection(Vector3 a2, Vector3 a1, Vector3 b2, Vector3 b1)
    {
        Vector3 intersection;
        Vector3 aDiff = a2 - a1;
        Vector3 bDiff = b2 - b1;
        if (LineLineIntersection(out intersection, a1, aDiff, b1, bDiff))
        {
            float aSqrMagnitude = aDiff.sqrMagnitude;
            float bSqrMagnitude = bDiff.sqrMagnitude;

            if ((intersection - a1).sqrMagnitude <= aSqrMagnitude
                 && (intersection - a2).sqrMagnitude <= aSqrMagnitude
                 && (intersection - b1).sqrMagnitude <= bSqrMagnitude
                 && (intersection - b2).sqrMagnitude <= bSqrMagnitude)
            {
                // there is an intersection between the two segments and it is at intersection
                return intersection;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }

    
    IEnumerator waitForScanSingle(float scanWaitTime)
    {

        yield return new WaitForSeconds(scanWaitTime);
        SimpleMethod();

    }
    static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
       Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
    private void SimpleMethod()
    {
        methodToCall();
    }


}