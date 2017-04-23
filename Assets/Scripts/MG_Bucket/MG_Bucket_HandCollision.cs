using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Bucket_HandCollision : MonoBehaviour {

    int toLayer;

    void Awake()
    {
        toLayer = LayerMask.NameToLayer("Isolated");
    }
    void OnCollisionEnter2D(Collision2D col)
    {

        MG_Bucket_Item item = col.gameObject.GetComponent<MG_Bucket_Item>();
        //  Debug.Log(item);
        if (item && item.speeding)
        {
            item.speeding = false;
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.up * 5 + Vector2.right * Random.Range(-2f, 2f);
            item.gameObject.layer = toLayer;
        }
    }
}
