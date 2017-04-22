using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LocalMinimum;
using UnityEngine.UI;

public class MiniGameLoader : Singleton<MiniGameLoader> {

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

    [SerializeField]
    Camera roomCamera;

    Color loadedColor;
    Color nonLoadedColor;

    RenderTexture rendTexture;

    void Load(string name)
    {
        if (!string.IsNullOrEmpty(currentMiniGameScene))
        {
            UnloadCurrent();
        }

        roomCamera.GetComponent<AudioListener>().enabled = false;
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        currentMiniGameScene = name;
        StartCoroutine(FadeIn());
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
        miniGameBG.color = loadedColor;
        miniGame.enabled = false;
        roomCamera.GetComponent<AudioListener>().enabled = true;

    }

    IEnumerator<WaitForSeconds> FadeIn()
    {

        yield return new WaitForSeconds(easeOutDuration);
        miniGameBG.raycastTarget = true;
        miniGame.raycastTarget = true;
        miniGame.enabled = true;
        miniGameBG.color = loadedColor;
        MiniGameCam.instance.SceneCamera.targetTexture = rendTexture;
    }

    void Start()
    {
        rendTexture = new RenderTexture(800, 500, 32);
        loadedColor = miniGameBG.color;
        nonLoadedColor = miniGameBG.color;
        nonLoadedColor.a = 0;
        miniGameBG.raycastTarget = false;
        miniGameBG.color = nonLoadedColor;
        miniGame.raycastTarget = false;
        miniGame.enabled = false;


        //Sprite rendSprite = Sprite.Create(rendTexture, new Rect(0, 0, rendTexture.width, rendTexture.height), Vector2.one * 0.5f);
        miniGame.material.mainTexture = rendTexture;

        StartCoroutine(LoadRND());
    }

    IEnumerator<WaitForSeconds> LoadRND()
    {
        yield return new WaitForSeconds(2f);
        LoadRandom();
    }
    
}
