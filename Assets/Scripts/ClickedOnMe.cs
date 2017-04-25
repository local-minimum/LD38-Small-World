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

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = mousePosition;
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(pos, clickLayer);
            if (hitColliders.Contains(col))
            {
                Clicked();
            } 
        }
    }

    Vector2 mousePosition
    {

        get
        {
            Vector2 pos;
            if (MiniGameLoader.instance.TargetRectMousePos(out pos))
            {
                return MiniGameCam.instance.SceneCamera.ViewportToWorldPoint(pos);
            } else
            {
                return Vector2.one * -1;
            }

        }
    }


    virtual protected void Clicked()
    {
        Debug.Log("Destroying " + name);
        MiniGameAudioEffector.instance.EmitRandomSound();
        Destroy(gameObject);
    }
}
