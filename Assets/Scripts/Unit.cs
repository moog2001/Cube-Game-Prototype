using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    private float m_health = 100.0f;
    public float health
    {
        get{ return m_health; }
        set 
        { 
            if(value >= 0.0f ){
                m_health = value;
            }
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0 ){
            Destroy(gameObject);
        }
    }
}
