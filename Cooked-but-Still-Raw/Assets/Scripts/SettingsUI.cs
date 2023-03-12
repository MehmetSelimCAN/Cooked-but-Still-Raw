using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Awake() {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void ChangeMusicVolume() {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        AudioManager.Instance.musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void ChangeSFXVolume() {
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        AudioManager.Instance.effectAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
    }
}
