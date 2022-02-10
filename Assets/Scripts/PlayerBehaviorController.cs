using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;

public class PlayerBehaviorController : MonoBehaviour

{
    bool test = false;
    [SerializeField] GameObject prefab;

    PlayerGridCreation playerGridCreation;
    // Start is called before the first frame update
    void Start()
    {
        playerGridCreation = GetComponent<PlayerGridCreation>();

    }
    public void Initiliaze()
    {
        print("thing");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !test)
        {
            for (int i = 0; i < playerGridCreation.gridCenterTransforms.GetLength(0); i++)
            {
                for (int j = 0; j < playerGridCreation.gridCenterTransforms[i].GetLength(0); j++)
                {
                    for (int n = 0; n < playerGridCreation.gridCenterTransforms[i].GetLength(1); n++)
                    {
                        SpawnPowerUp(playerGridCreation.gridCenterTransforms[i][j, n].position, prefab);
                        Debug.DrawRay(transform.TransformPoint(playerGridCreation.gridCenterTransforms[i][j, n].position*1.2f), transform.position - transform.TransformPoint(playerGridCreation.gridCenterTransforms[i][j, n].position*1.2f));
                    }
                }
            }

        }

    }
    void SpawnPowerUp(Vector3 position, GameObject prefab)
    {
        Vector3 rayStartPosition = position * 1.001f;
        RaycastHit hit;
        Physics.Raycast(transform.TransformPoint(rayStartPosition), transform.position - transform.TransformPoint(rayStartPosition), out hit, 0.2f);
        GameObject powerUpObject = Instantiate(prefab, transform.TransformPoint(position), prefab.transform.rotation);
        powerUpObject.transform.parent = transform;
        powerUpObject.transform.rotation = Quaternion.LookRotation(hit.normal, -Vector3.up) * Quaternion.AngleAxis(90f, Vector3.right);

    }



}
