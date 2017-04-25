using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public enum Controllers { Mouse, Keyboard};

public abstract class MiniGamePlayerBase : Singleton<MiniGamePlayerBase> {

    public Controllers[] controllers = new Controllers[] { Controllers.Mouse, Controllers.Keyboard };

    [SerializeField]
    protected float m_Timeout = 5.0f;
    protected float m_TimeLeft = 0.0f;

    public float ProgressToSilence
    {
        get
        {
            return m_TimeLeft / m_Timeout;
        }
    }

    public abstract void Play(int difficulty);

    [SerializeField]
    bool autoPlay = false;

    private bool m_ready = false;


    public bool ReadyToPlay
    {
        get
        {
            return m_ready;
        }
    }

    private void Start()
    {

        if (autoPlay)
        {
            gameObject.AddComponent<AudioListener>();
            StartCoroutine(DelayPlay());
        }
        else {
            m_ready = true;
        }
    }

    IEnumerator<WaitForSeconds> DelayPlay()
    {
        yield return new WaitForSeconds(1f);
        Play(1);
    }

    protected bool m_Playing = false;

    public bool Playing
    {
        get { return m_Playing; }

        set {
            if (!value)
            {
                m_Playing = false;
            }
        }
    }

    public abstract void EndGame();    

    public void Update()
    {
        if (m_Playing)
        {
            m_TimeLeft -= Time.deltaTime;
            if (m_TimeLeft < 0)
            {
                Debug.Log("Timeout");
                EndGame();
                //Important that m_Playing = false is after so EndGame can also check state
                m_Playing = false;
            }
        }
    }
}
