using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class MiniGameAudioEffector : Singleton<MiniGameAudioEffector> {

    AudioSource speaker;
    [SerializeField]
    AudioClip music;

    [SerializeField]
    AudioClip[] sounds;

    [SerializeField]
    AudioClip[] selectSounds;

    void Start()
    {
        speaker = GetComponent<AudioSource>();
        SpeakerSystem.instance.FadeToMiniGame(music);
    }

    [SerializeField]
    float timeBetweenEmits = 0.15f;

    public void EmitRandomSound()
    {
        SpeakerSystem.instance.PlayMinigameEffect(sounds[Random.Range(0, sounds.Length)], timeBetweenEmits);
    }

    public void EmitRandomSelectSound()
    {
        SpeakerSystem.instance.PlayMinigameEffect(selectSounds[Random.Range(0, selectSounds.Length)], 0);
    }


}
