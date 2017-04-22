using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LocalMinimum;
using UnityEngine.UI;

public class MiniGameLoader : Singleton<MiniGameLoader>{

    [SerializeField]
    string[] SceneNames;

    string currentMiniGameScene;

    [SerializeField]
    Image miniGameBG;

    [SerializeField]
    Image miniGame;

    [SerializeField]
    AnimationCurve easeOut;

    [SerializeField]
    float easeOutDuration;

   
    void Load(string name)
    {
        if (string.IsNullOrEmpty(currentMiniGameScene))
        {
            UnloadCurrent();
        }

        StartCoroutine(FadeIn());
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        currentMiniGameScene = name;
    }

    public void LoadRandom()
    {
        Load(SceneNames[Random.Range(0, SceneNames.Length)]);
    }

    void UnloadCurrent()
    {
        StartCoroutine(FadeOut());
        SceneManager.UnloadSceneAsync(currentMiniGameScene);        
    }

    IEnumerator<WaitForSeconds> FadeOut()
    {
        miniGameBG.raycastTarget = false;
        miniGame.raycastTarget = false;
        yield return new WaitForSeconds(easeOutDuration);
        miniGameBG.enabled = false;
    }

    IEnumerator<WaitForSeconds> FadeIn()
    {
        miniGameBG.enabled = true;
        yield return new WaitForSeconds(easeOutDuration);
        miniGameBG.raycastTarget = true;
        miniGame.raycastTarget = true;
    }
}
