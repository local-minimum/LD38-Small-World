using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Credits : MonoBehaviour {

    [SerializeField]
    string replayScene;

    public void Replay()
    {
        SceneManager.LoadScene(replayScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
