using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class HealthUI : Singleton<HealthUI> {

    static string HealthData = "Player.Health";

    [SerializeField]
    string talkTrigger = "Talk";

    [SerializeField]
    Animator[] mouths;

    [SerializeField]
    GameObject hider;

    public void ShowHealthBar()
    {
        hider.SetActive(true);
    }

    public void HideHealthBar()
    {
        hider.SetActive(false);
    }

    static public int Health
    {
        get
        {
            return PlayerPrefs.GetInt(HealthData, -1);
        }

        private set
        {
            PlayerPrefs.SetInt(HealthData, value);
        }
    }

    public static bool IsDead
    {
        get
        {
            return Health >= 4;
        }
    }

    public static void ResetHealth()
    {
        Health = -1;
    }

    public void Increase()
    {
        int value = Health + 1;
        if (value < mouths.Length)
        {
            Health = value;
            mouths[value].SetTrigger(talkTrigger);
        } else
        {
            Health = 4;
        }
    }

    void Start()
    {
        StartCoroutine(AnimateResync());
    }

    [SerializeField]
    float delay = 0.5f;

    IEnumerator<WaitForSeconds> AnimateResync()
    {
        int i = 0;
        int value = Health;
        while (i <= value && i < 4)
        {
            yield return new WaitForSeconds(delay);
            mouths[i].SetTrigger(talkTrigger);
            i++;
        }
    }

}
