using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Bucket_Hand : MonoBehaviour {

    [SerializeField]
    MG_Bucket_Hand left;

    [SerializeField]
    MG_Bucket_Hand right;

    [SerializeField]
    GameObject hand;

    public void HandOn()
    {
        isOn = true;
        hand.SetActive(true);
    }

    public void HandOff()
    {
        isOn = false;
        hand.SetActive(false);
    }

    [SerializeField]
    bool startOn = false;
    bool isOn;

    void Awake()
    {
        if (startOn)
        {
            HandOn();
        } else
        {
            HandOff();
        }
    }

    static float lastUpdate;

    [SerializeField]
    float speed = 0.1f;

    void Update()
    {
        if (isOn && Input.GetButtonDown("Horizontal") && Time.realtimeSinceStartup - lastUpdate > speed)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (right)
                {
                    HandOff();
                    right.HandOn();
                    lastUpdate = Time.realtimeSinceStartup;
                } 
            } else if (left)
            {
                HandOff();
                left.HandOn();
                lastUpdate = Time.realtimeSinceStartup;
            }
        }
    }
}
