using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorController : MonoBehaviour

{
    bool test = false;
    [SerializeField] GameObject testObject;

    PlayerGridCreation playerGridCreation;
    // Start is called before the first frame update
    void Start()
    {
        playerGridCreation = GetComponent<PlayerGridCreation>();
    }
    public void Initiliaze()
    {
        print("thing");
        print(playerGridCreation.gridCenterTransforms[4][0, 0].position);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !test)
        {
            GameObject thing = Instantiate(testObject, transform.TransformPoint(playerGridCreation.gridCenterTransforms[4][0, 0].position), testObject.transform.rotation);
            thing.transform.parent = gameObject.transform;
            test = true;
        }

    }
}
