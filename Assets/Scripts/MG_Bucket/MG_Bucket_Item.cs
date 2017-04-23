using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Bucket_Item : MonoBehaviour {
    public float speed = 2;
    public ConversationPiece piece;
    public bool speeding = true;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (speeding)
        {
            rb.velocity = Vector2.down * speed;
        }
    }
}
