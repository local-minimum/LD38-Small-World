using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_BurpItem : MonoBehaviour {

    public float maxSpeed = 2;
    public float minSpeed = 1;
    Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();	
	}
	
	
	void Update () {
        Vector2 vel = rb.velocity;
        rb.velocity = vel.normalized * Mathf.Clamp(vel.magnitude, minSpeed, maxSpeed);
	}
}
