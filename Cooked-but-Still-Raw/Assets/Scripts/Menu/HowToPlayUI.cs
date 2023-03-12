using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour {

    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private List<Transform> pages;
    private int currentPageIndex = 0;

    private void Awake() {
        backToMenuButton.onClick.AddListener(() => {
            CloseAllPages();

            currentPageIndex = 0;
            pages[0].gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);
            previousPageButton.gameObject.SetActive(false);

            MainMenuManager.Instance.EnableMainMenuUI();
        });

        nextPageButton.onClick.AddListener(() => {
            CloseAllPages();

            currentPageIndex++;

            if (currentPageIndex == 1) {
                previousPageButton.gameObject.SetActive(true);
            }

            if (currentPageIndex == pages.Count - 1) {
                nextPageButton.gameObject.SetActive(false);
            }

            pages[currentPageIndex].gameObject.SetActive(true);
        });

        previousPageButton.onClick.AddListener(() => {
            CloseAllPages();

            currentPageIndex--;

            if (currentPageIndex == 0) {
                previousPageButton.gameObject.SetActive(false);
            }

            if (currentPageIndex == pages.Count - 2) {
                nextPageButton.gameObject.SetActive(true);
            }

            pages[currentPageIndex].gameObject.SetActive(true);
        });
    }

    private void CloseAllPages() {
        for (int i = 0; i < pages.Count; i++) {
            pages[i].gameObject.SetActive(false);
        }
    }
}
