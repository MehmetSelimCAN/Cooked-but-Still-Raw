using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager> {

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerFillImage;
    private int levelTime;

    private Animator UIManagerAnimator;
    [SerializeField] private TextMeshProUGUI currentCoinText;
    [SerializeField] private TextMeshProUGUI incomingCoinText;

    private void Start() {
        UIManagerAnimator = GetComponent<Animator>();
        levelTime = GameController.Instance.LevelTime;
    }

    public void UpdateTimerUI(float remainingTime) {
        int minutes = (int)(remainingTime / 60);
        int seconds = (int)(remainingTime % 60);

        timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));

        timerFillImage.fillAmount = remainingTime / levelTime;
    }

    public void UpdateIncomingCoinUI(int incomingCoinCount) {
        if (incomingCoinCount > 0) {
            UIManagerAnimator.Play("IncomingCoin", 0, 0);
            incomingCoinText.SetText("+" + incomingCoinCount);
        }
        else {
            UIManagerAnimator.Play("IncomingPenalty", 0, 0);
            incomingCoinText.SetText(incomingCoinCount.ToString());
        }

    }

    public void UpdateCurrentCoinUI() {
        currentCoinText.SetText(GameController.Instance.CurrentCoinCount.ToString());
    }

    public void EnableEndGameScreen() {

    }
}
