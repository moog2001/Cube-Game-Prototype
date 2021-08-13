using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject parentObject;
    private RaycastHit hit;
    public string collisionObjectName;

    public List<Vector3> hitPoints = new List<Vector3>();
    public bool isDone = false;
    public IEnumerator FollowVector3Points(float time, Vector3[] travelPositions)
    {
        for (int i = 0; i < travelPositions.Length - 1; i++)
        {
            Vector3 startingPos = travelPositions[i];
            Vector3 finalPos = travelPositions[i + 1];
            transform.position = startingPos;
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }
        PlayerGridCreation playerGridCreation = parentObject.GetComponent<PlayerGridCreation>();
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == collisionObjectName)
        {
            hitPoints.Add(other.contacts[0].point);
        }
    }
}

