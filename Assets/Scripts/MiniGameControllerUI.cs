using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LocalMinimum;

public class MiniGameControllerUI : Singleton<MiniGameControllerUI> {


    [SerializeField]
    GameObject mouseIcon;

    [SerializeField]
    GameObject keyboardIcon;

    [SerializeField]
    GameObject bothIcon;

    [SerializeField]
    GameObject bg;

    public void HideAll()
    {
        bg.SetActive(false);
        mouseIcon.SetActive(false);
        keyboardIcon.SetActive(false);
        bothIcon.SetActive(false);
    }

    public void Show(Controllers[] controllers)
    {
        if (controllers.Length == 2)
        {
            mouseIcon.SetActive(false);
            keyboardIcon.SetActive(false);
            bothIcon.SetActive(true);
        }
        else {
            mouseIcon.SetActive(controllers.Contains(Controllers.Mouse));
            keyboardIcon.SetActive(controllers.Contains(Controllers.Keyboard));
            bothIcon.SetActive(false);
        }
        bg.SetActive(true);
    }
}
