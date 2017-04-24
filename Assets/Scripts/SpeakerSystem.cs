using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;
using UnityEngine.Audio;

public class SpeakerSystem : Singleton<SpeakerSystem> {

    [SerializeField]
    AudioSource miniGameEffectSpeaker;

    [SerializeField]
    AudioSource miniGameMusicSpeaker;

    [SerializeField]
    AudioSource worldEffectSpeaker;

    [SerializeField]
    AudioSource worldMusicSpeaker;

    void Start () {
        DontDestroyOnLoad(gameObject);
        FadeToWorld();
	}

    [SerializeField]
    AudioClip[] blablas;

    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    AudioMixerSnapshot[] worldSnap;

    [SerializeField]
    AudioMixerSnapshot[] miniSnap;

    [SerializeField]
    float snapTransition =0.5f;

    public void FadeToWorld()
    {
        if (!Muted)
        {
            mixer.TransitionToSnapshots(worldSnap, new float[] { 1 }, snapTransition);
        } 
        unMute = worldSnap;
        
    }

    public void FadeToMiniGame(AudioClip music)
    {
        miniGameMusicSpeaker.Stop();
        miniGameMusicSpeaker.clip = music;
        miniGameMusicSpeaker.Play();
        if (!Muted)
        {
            mixer.TransitionToSnapshots(miniSnap, new float[] { 1 }, snapTransition);
        }
        unMute = miniSnap;
        
    }


    float lastRefuseMinigame = -999f;

    public bool PlayMinigameEffect(AudioClip clip, float refuseDuration)
    {
        if (Time.timeSinceLevelLoad - lastRefuseMinigame > refuseDuration)
        {
            lastRefuseMinigame = Time.timeSinceLevelLoad;
            miniGameEffectSpeaker.PlayOneShot(clip);
            return true;
        }
        return false;
    }

    [SerializeField]
    float blaRefuseDuration = 0.3f;

    float lastBla = -999f;

    public bool Bla()
    {
        if (Time.timeSinceLevelLoad - lastBla > blaRefuseDuration)
        {
            lastBla = Time.timeSinceLevelLoad;
            worldEffectSpeaker.PlayOneShot(blablas[Random.Range(0, blablas.Length)]);
            return true;
        }
        return false;

    }

    [SerializeField]
    AudioMixerSnapshot[] muteSnap;

    bool muted = false;
    AudioMixerSnapshot[] unMute;

    public bool Muted
    {
        set
        {
            if (value)
            {
                mixer.TransitionToSnapshots(muteSnap, new float[] { 1 }, snapTransition);
            } else
            {
                mixer.TransitionToSnapshots(unMute, new float[] { 1 }, snapTransition);
            }
            muted = value; 
        }

        get
        {
            return muted;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Muted = !Muted;
        }
    }
}
