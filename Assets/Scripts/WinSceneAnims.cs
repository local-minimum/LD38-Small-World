using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneAnims : MonoBehaviour {

    [SerializeField]
    Animator[] frenemies;

    [SerializeField]
    string enterTrigger;

    [SerializeField]
    float[] entryAt;

    [SerializeField]
    GameObject message;

    [SerializeField]
    float messageShowAt;

    [SerializeField]
    string credScene = "Credits";

    [SerializeField]
    float loadCredAt;
	void Start () {
		for (int i=0; i<frenemies.Length; i++)
        {
            StartCoroutine(DelayEntry(frenemies[i], entryAt[i]));
        }

        StartCoroutine(ShowWelcome());
        StartCoroutine(LoadCred());
	}

    IEnumerator<WaitForSeconds> DelayEntry(Animator anim, float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetTrigger(enterTrigger);
        Debug.Log(Time.timeSinceLevelLoad + " entered frenemy " + anim.gameObject.name);
    }

	IEnumerator<WaitForSeconds> ShowWelcome()
    {
        yield return new WaitForSeconds(messageShowAt);
        message.SetActive(true);
    }

    IEnumerator<WaitForSeconds> LoadCred()
    {
        yield return new WaitForSeconds(loadCredAt);
        SceneManager.LoadScene(credScene);
    }
}
    