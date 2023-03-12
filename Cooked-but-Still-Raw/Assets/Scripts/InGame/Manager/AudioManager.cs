using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] public AudioSource musicAudioSource;
    [SerializeField] public AudioSource effectAudioSource;

    [SerializeField] private AudioClip droppingItemClipAudio;
    [SerializeField] protected AudioClip pickingItemClipAudio;
    [SerializeField] protected AudioClip cuttingClipAudio;
    [SerializeField] protected AudioClip washingClipAudio;
    [SerializeField] protected AudioClip soupPourClipAudio;
    [SerializeField] protected AudioClip meatAddingClipAudio;

    public override void Awake() {
        base.Awake();
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        effectAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void PlayBackgroundMusic() {
        musicAudioSource.Play();
    }

    public void StopBackgroundMusic() {
        musicAudioSource.Stop();
    }

    public void PlayDroppingItemAudio() {
        effectAudioSource.clip = droppingItemClipAudio;
        effectAudioSource.Play();
    }

    public void PlayPickingItemAudio() {
        effectAudioSource.clip = pickingItemClipAudio;
        effectAudioSource.Play();
    }

    public void PlayCuttingAudio() {
        effectAudioSource.clip = cuttingClipAudio;
        effectAudioSource.Play();
    }

    public void PlayWashingAudio() {
        effectAudioSource.clip = washingClipAudio;
        effectAudioSource.Play();
    }

    public void PlaySoupPourAudio() {
        effectAudioSource.clip = soupPourClipAudio;
        effectAudioSource.Play();
    }

    public void PlayMeatAddingAudio() {
        effectAudioSource.clip = meatAddingClipAudio;
        effectAudioSource.Play();
    }
}
