using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class MenuManager : Singleton<MenuManager> {

    [SerializeField] private Transform mainMenuUI;
    [SerializeField] private Transform settingsUI;
    [SerializeField] private Transform howToPlayUI;

    [SerializeField] private Volume globalVolume;
    private Vignette vignette;
    [SerializeField] private float fadeSpeed;

    public override void Awake() {
        base.Awake();
        Time.timeScale = 1;
        AudioManager.Instance.PlayBackgroundMusic();
    }

    public void PlayGame() {
        globalVolume.profile.TryGet(out vignette);

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() {
        while (vignette.intensity.value < 1) {
            vignette.intensity.value += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        SceneManager.LoadScene("GameScene");
    }

    public void EnableMainMenuUI() {
        mainMenuUI.gameObject.SetActive(true);
        settingsUI.gameObject.SetActive(false);
        howToPlayUI.gameObject.SetActive(false);
    }

    public void EnableSettingsUI() {
        settingsUI.gameObject.SetActive(true);
        mainMenuUI.gameObject.SetActive(false);
        howToPlayUI.gameObject.SetActive(false);
    }

    public void EnableHowToPlayUI() {
        howToPlayUI.gameObject.SetActive(true);
        settingsUI.gameObject.SetActive(false);
        mainMenuUI.gameObject.SetActive(false);
    }
}
