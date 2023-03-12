using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour, IMenuScreen {

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button backButton;

    private void Awake() {
        backButton.onClick.AddListener(() => {
            Hide();
        });
    }

    public void ChangeMusicVolume() {
        //PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void ChangeSFXVolume() {
        //PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
    }

    public void Show() {
        MainMenuManager.Instance.EnableUI(transform);
    }

    public void Hide() {
        MainMenuManager.Instance.DisableUI(transform);
    }
}
