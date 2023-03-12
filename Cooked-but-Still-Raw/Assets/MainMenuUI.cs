using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour, IMenuScreen {

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button howToPlayButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            MainMenuManager.Instance.PlayGame();
        });

        settingsButton.onClick.AddListener(() => {
            MainMenuManager.Instance.EnableSettingsUI();
        });

        howToPlayButton.onClick.AddListener(() => {
            MainMenuManager.Instance.EnableHowToPlayUI();
        });
    }

    public void Show() {
        MainMenuManager.Instance.EnableUI(transform);
    }

    public void Hide() {
        MainMenuManager.Instance.DisableUI(transform);
    }
}
