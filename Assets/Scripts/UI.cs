using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text health;
    GameObject player;
    Unit playerUnit;
    void Start()
    {
        player = GameObject.Find("Player");
        playerUnit = player.GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        health.text = "Health: " + playerUnit.health;
    }
}
