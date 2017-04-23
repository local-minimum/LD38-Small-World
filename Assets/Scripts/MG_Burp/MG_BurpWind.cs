using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_BurpWind : MonoBehaviour {

    public float force = 10f;
    public bool burping = true;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<MG_BurpItem>() != null)
        {
            Vector3 forceV = transform.up * force;
            other.GetComponent<Rigidbody2D>().AddForce(forceV, ForceMode2D.Force);
        }
    }
}
