using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClickedOnMe : MonoBehaviour {

    Collider2D col;

    [SerializeField]
    LayerMask clickLayer;

    
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition, clickLayer);
            if (hitColliders.Contains(col))
            {
                Clicked();
            } 
        }
    }

    virtual protected void Clicked()
    {
        Debug.Log("Destroying " + name);
        Destroy(gameObject);
    }
}
