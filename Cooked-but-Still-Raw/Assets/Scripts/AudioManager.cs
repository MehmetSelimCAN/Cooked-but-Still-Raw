using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    public void PlayEffectAudio(AudioClip clip) {
        effectAudioSource.clip = clip;
        effectAudioSource.Play();
    }
}
