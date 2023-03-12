using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct UIsRelations {
    [SerializeField] public Transform parentTransform;
    [SerializeField] public List<Transform> childTransforms;
}

public class MainMenuManager : Singleton<MainMenuManager> {

    [SerializeField] private List<UIsRelations> UIsRelations;

    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private Transform howToPlayUI;

    [SerializeField] private Volume globalVolume;
    private Vignette vignette;
    [SerializeField] private float fadeSpeed;

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

    public void EnableUI(Transform UI_ToBeEnabled) {
        foreach (UIsRelations UIsRelation in UIsRelations) {
            if (UIsRelation.parentTransform == UI_ToBeEnabled) {
                UIsRelation.parentTransform.gameObject.SetActive(true);

                foreach (Transform child in UIsRelation.parentTransform) {
                    child.gameObject.SetActive(false);
                }
                break;
            }
        }
    }

    public void DisableUI(Transform UI_ToBeDisabled) {
        UI_ToBeDisabled.gameObject.SetActive(false);
        foreach (UIsRelations UIsRelation in UIsRelations) {
            foreach (Transform child in UIsRelation.parentTransform) {
                if (child == UI_ToBeDisabled) {
                    UIsRelation.parentTransform.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void EnableMainMenuUI() {
        settingsUI.gameObject.SetActive(false);
        howToPlayUI.gameObject.SetActive(false);
    }

    public void EnableSettingsUI() {
        mainMenuUI.GetComponent<IMenuScreen>().Hide();
    }

    public void EnableHowToPlayUI() {
        howToPlayUI.gameObject.SetActive(true);
    }

    public void OpenHowToPlayFirstPage() {

    }

    public void OpenHowToPlaySecondPage() {

    }

    public void OpenHowToPlayThirdPage() {

    }
}
