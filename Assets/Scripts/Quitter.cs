using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quitter : MonoBehaviour {

    enum QuitState { Playing, Asking, Quitting};

    QuitState state = QuitState.Playing;

    Image maskImage;

	void Update () {
	    if (state == QuitState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Pause))
            {
                StopPlay();
            }
        } else if (state == QuitState.Asking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!allowQuit || Application.isWebPlayer)
                {
                    ResumePlay();
                }
                else {
                    Debug.Log("Good Bye");
                    state = QuitState.Quitting;
                    Application.Quit();
                }

            } else if (Input.anyKeyDown)
            {
                ResumePlay();        
            }
        }
	}


    float playTimeScale;

    void StopPlay()
    {
        playTimeScale = Time.timeScale;
        Time.timeScale = 0;
        state = QuitState.Asking;
        maskImage.color = showingColor;
        maskImage.raycastTarget = true;
    }

    void ResumePlay()
    {
        Time.timeScale = playTimeScale;
        state = QuitState.Playing;
        maskImage.color = hidingColor;
        maskImage.raycastTarget = false;
    }

    Color hidingColor;
    Color showingColor;

    bool allowQuit = false;

    void Start()
    {
        if (!allowQuit || Application.isWebPlayer)
        {
            GetComponentInChildren<Text>().text = "Press any button to resume...";
        }
        maskImage = GetComponent<Image>();
        showingColor = maskImage.color;
        hidingColor = maskImage.color;
        hidingColor.a = 0;
        maskImage.color = hidingColor;
        maskImage.raycastTarget = false;
    }
}
