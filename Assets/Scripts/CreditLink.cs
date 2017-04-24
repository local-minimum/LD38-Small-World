using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditLink : MonoBehaviour {

    [SerializeField]
    string linkURI;

    public void ClickLink()
    {
        if (!string.IsNullOrEmpty(linkURI))
        {
            Application.OpenURL(linkURI);
        }
    }
}
