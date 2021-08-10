using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConstantDamage : Unit
{
    // Start is called before the first frame update
    GameObject contactObject;
    Unit contactObjectScript;
    public float constantDamageAmount;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ally"))
        {

            contactObject = other.gameObject;
            contactObjectScript = contactObject.GetComponent<Unit>();
            StartCoroutine(ConstantDamage(constantDamageAmount));
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ally"))
        {

            StopCoroutine(ConstantDamage(constantDamageAmount));
            contactObject = null;
            contactObjectScript = null;

        }
    }
    IEnumerator ConstantDamage(float damageAmount)
    {
        contactObjectScript.health -= damageAmount;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(ConstantDamage(constantDamageAmount));

    }
}
