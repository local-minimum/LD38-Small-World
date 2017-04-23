using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilenceUI : MonoBehaviour {
    [SerializeField]
    Image progress;

    [SerializeField]
    GameObject hider;

    void Update()
    {
        bool showing = MiniGamePlayerBase.IsInstanciated && MiniGamePlayerBase.instance.Playing;

        if (showing)
        {
            progress.fillAmount = 1 - MiniGamePlayerBase.instance.ProgressToSilence;
            hider.SetActive(true);
        } else
        {
            hider.SetActive(false);
        }
    }
}
