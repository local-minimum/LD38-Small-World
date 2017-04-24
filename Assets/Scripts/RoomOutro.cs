using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LocalMinimum;
using UnityEngine.UI;


public class RoomOutro : Singleton<RoomOutro> {

    bool displaying;

    Action _callback;

    [SerializeField]
    GameObject outro;

    public void Hide()
    {
        outro.SetActive(false);
    }

    public void ShowOutro(Action callback)
    {
        _callback = callback;
        displaying = true;
        outro.SetActive(true);
    }

    public void Continue()
    {
        if (displaying)
        {
            displaying = false;
            outro.GetComponentInChildren<Button>().gameObject.SetActive(false);
            
            if (_callback != null)
            {
                _callback();
            }
        }
    }

    void Update()
    {
        if (displaying)
        {
            if (Input.GetButtonDown("Submit"))
            {
                Continue();
            }
        }
    }

}
