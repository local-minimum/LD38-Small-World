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
        mixer.TransitionToSnapshots(worldSnap, new float[] { 1 }, snapTransition);
    }

    public void FadeToMiniGame(AudioClip music)
    {
        miniGameMusicSpeaker.Stop();
        miniGameMusicSpeaker.clip = music;
        miniGameMusicSpeaker.Play();
        mixer.TransitionToSnapshots(miniSnap, new float[] { 1 }, snapTransition);
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
}
