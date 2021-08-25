using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject parentObject;
    private RaycastHit hit;
    public List<Vector3> hitPoints = new List<Vector3>();
    public bool isDone = false;

    private void Start()
    {
        transform.SetParent(parentObject.transform);
        StartCoroutine(NormalCheck());
    }
    public IEnumerator FollowVector3Points(float time, Vector3[] travelPositions)
    {
        for (int i = 0; i < travelPositions.Length - 1; i++)
        {
            Vector3 startingPos = travelPositions[i];
            Vector3 finalPos = travelPositions[i + 1];
            float elapsedTime = 0;
            
            while (elapsedTime < time)
            {
                transform.Rotate(Vector3.up* 500f * Time.deltaTime,Space.Self);
                transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

        }
        Destroy(gameObject);
    }

    IEnumerator NormalCheck()
    {
        while (gameObject != null)
        {
            if (Physics.Raycast(transform.position, parentObject.transform.position - transform.position, out hit, 3f) && hit.collider.gameObject == parentObject)
            {

                gameObject.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                

            }



            yield return new WaitForSeconds(0.1f);
        }
    }

}

