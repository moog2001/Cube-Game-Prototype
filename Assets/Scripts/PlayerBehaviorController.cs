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
            SpawnPowerUp(playerGridCreation.gridCenterTransforms[0][0, 0].position, prefab);
            SpawnPowerUp(playerGridCreation.gridCenterTransforms[1][0, 0].position, prefab);
            SpawnPowerUp(playerGridCreation.gridCenterTransforms[2][0, 0].position, prefab);
            SpawnPowerUp(playerGridCreation.gridCenterTransforms[3][0, 0].position, prefab);
            SpawnPowerUp(playerGridCreation.gridCenterTransforms[4][0, 0].position, prefab);
        }

    }
    void SpawnPowerUp(Vector3 position, GameObject prefab)
    {
        Vector3 rayStartPosition = position * 1.1f;
        RaycastHit hit;
        Physics.Raycast(transform.TransformPoint(rayStartPosition), transform.position - transform.TransformPoint(rayStartPosition), out hit, 0.2f);
        GameObject powerUpObject = Instantiate(prefab, transform.TransformPoint(position), prefab.transform.rotation);
        powerUpObject.transform.parent = transform;
        powerUpObject.transform.rotation = Quaternion.LookRotation(hit.normal, -Vector3.up) * Quaternion.AngleAxis(90f, Vector3.right);

    }



}
