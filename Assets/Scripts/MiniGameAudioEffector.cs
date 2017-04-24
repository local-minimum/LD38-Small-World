using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalMinimum;

public class MiniGameAudioEffector : Singleton<MiniGameAudioEffector> {

    AudioSource speaker;

    [SerializeField]
    AudioClip[] sounds;

    [SerializeField]
    float refuseDuration = 0.4f;

    float lastEmit = -999f;

    void Start()
    {
        speaker = GetComponent<AudioSource>();
    }

    public void EmitRandomSound()
    {
        if (Time.timeSinceLevelLoad - lastEmit > refuseDuration)
        {
            lastEmit = Time.timeSinceLevelLoad;
            speaker.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
        }
    }

    public void EmitSound(AudioClip sound)
    {
        if (Time.timeSinceLevelLoad - lastEmit > refuseDuration)
        {
            lastEmit = Time.timeSinceLevelLoad;
            speaker.PlayOneShot(sound);
        }
    }
}
