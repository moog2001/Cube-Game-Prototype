using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vectorDivision : MonoBehaviour
{
    // Start is called before the first frame updatepub
    public GameObject startObject;
    public GameObject endObject;
    public int gridNum;
    int divideNumber;
    Vector3[] centerOuts;


    void Start()
    {
        divideNumber = gridNum * 2;
        centerOuts = VectorDivide(startObject.transform.position, endObject.transform.position, divideNumber);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Vector3 centerOut in centerOuts)
        {
            Debug.DrawRay(centerOut, transform.up, Color.blue, 1f);
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

}
