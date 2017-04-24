using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeIntroScene : MonoBehaviour {

    [SerializeField]
    float timeBeforeBG;

    [SerializeField]
    GameObject bgField;

    [SerializeField]
    Text title;

    [SerializeField]
    float delayTitle;

    [SerializeField]
    Text subTitle;

    [SerializeField]
    float delaySubtitle;

    [SerializeField]
    float delayIntroText;

    void Start()
    {
        HealthUI.ResetHealth();
        StartCoroutine(RunIntro());
    }

    IEnumerator<WaitForSeconds> RunIntro()
    {
        SpeakerSystem.instance.FadeToWorld();
        bgField.SetActive(false);
        Color titleColor = title.color;
        title.color = new Color(titleColor.r, titleColor.g, titleColor.b, 0);
        Color subTitleColor = subTitle.color;
        subTitle.color = new Color(subTitleColor.r, subTitleColor.g, subTitleColor.b, 0);

        yield return new WaitForSeconds(timeBeforeBG);
        bgField.SetActive(true);
        yield return new WaitForSeconds(delayTitle);
        title.color = titleColor;
        yield return new WaitForSeconds(delaySubtitle);
        subTitle.color = subTitleColor;
        yield return new WaitForSeconds(delayIntroText);
        RoomOutro.instance.ShowOutro(ReadyToPlay);
        title.enabled = false;
        subTitle.enabled = false;
        bgField.SetActive(false);
    }

    public void ReadyToPlay()
    {
        RoomOutro.instance.Hide();
        StartCoroutine(LoadNext());
    }
    [SerializeField]
    float preTitlesRemove;

    [SerializeField]
    float preDoorOpen;

    [SerializeField]
    Animator anim;

    [SerializeField]
    string doorTrigger;

    [SerializeField]
    float loadDelay;

    [SerializeField]
    string nextScene;

    IEnumerator<WaitForSeconds> LoadNext()
    {
        yield return new WaitForSeconds(preDoorOpen);
        anim.SetTrigger(doorTrigger);
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(nextScene);
    }
}
