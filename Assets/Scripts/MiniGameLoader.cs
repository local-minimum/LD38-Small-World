﻿using System.Collections;
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

    public bool TargetRectMousePos(out Vector2 pos)
    {

        Vector2 screenPos = Input.mousePosition;
        Rect miniRect = RectTransformToScreenSpace(miniGame.rectTransform);
        if (miniRect.Contains(screenPos))
        {
            pos = screenPos - miniRect.min;
            pos.x /= miniRect.width;
            pos.y /= miniRect.height;
            return true;
        } else
        {
            Debug.Log(screenPos + " outside " + miniRect);
            pos = Vector2.zero;
            return false;
        }
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }

    void Load(string name)
    {
        if (!string.IsNullOrEmpty(currentMiniGameScene))
        {
            UnloadCurrent();
        }

        //roomCamera.GetComponent<AudioListener>().enabled = false;
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        currentMiniGameScene = name;
        StartCoroutine(FadeIn());
    }

    [SerializeField]
    string deathScene = "Death";

    public void LoadDeath()
    {
        Load(deathScene);
    }

    public void LoadRandom()
    {
        Load(SceneNames[Random.Range(0, SceneNames.Length)]);
    }

    public void UnloadCurrent()
    {
        if (!string.IsNullOrEmpty(currentMiniGameScene))
        {
            StartCoroutine(FadeOut());
            SceneManager.UnloadSceneAsync(currentMiniGameScene);
            currentMiniGameScene = null;
        }
    }

    IEnumerator<WaitForSeconds> FadeOut()
    {
        miniGameBG.raycastTarget = false;
        miniGame.raycastTarget = false;
        
        yield return new WaitForSeconds(easeOutDuration);
        miniGameBG.color = nonLoadedColor;
        miniGame.enabled = false;
        //roomCamera.GetComponent<AudioListener>().enabled = true;

    }

    IEnumerator<WaitForSeconds> FadeIn()
    {
        miniGameBG.raycastTarget = true;
        miniGame.raycastTarget = true;
        miniGame.enabled = true;
        miniGameBG.color = loadedColor;

        bool ready = false;
        while (!ready)
        {
            yield return new WaitForSeconds(0.02f);
            ready = MiniGamePlayerBase.IsInstanciated && MiniGamePlayerBase.instance.ReadyToPlay;
        }
        MiniGameCam.instance.SceneCamera.targetTexture = rendTexture;
        MiniGameControllerUI.instance.Show(MiniGamePlayerBase.instance.controllers);
        yield return new WaitForSeconds(easeOutDuration);
        MiniGamePlayerBase.instance.Play(Room.instance.Difficulty);
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

        miniGameBG.gameObject.SetActive(true);

        //Sprite rendSprite = Sprite.Create(rendTexture, new Rect(0, 0, rendTexture.width, rendTexture.height), Vector2.one * 0.5f);
        miniGame.material.mainTexture = rendTexture;
    }

    
}
