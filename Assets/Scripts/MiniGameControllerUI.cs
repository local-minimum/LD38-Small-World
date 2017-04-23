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

    public void HideAll()
    {
        mouseIcon.SetActive(false);
        keyboardIcon.SetActive(false);
    }

    public void Show(Controllers[] controllers)
    {
        mouseIcon.SetActive(controllers.Contains(Controllers.Mouse));
        keyboardIcon.SetActive(controllers.Contains(Controllers.Keyboard));
    }
}
