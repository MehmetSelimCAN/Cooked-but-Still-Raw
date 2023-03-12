using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager> {

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerFillImage;
    private int levelTime;

    private Animator UIManagerAnimator;
    [SerializeField] private TextMeshProUGUI currentCoinText;
    [SerializeField] private TextMeshProUGUI incomingCoinText;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI correctOrderText;
    [SerializeField] private TextMeshProUGUI wrongOrderText;
    [SerializeField] private TextMeshProUGUI missOrderText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public override void Awake() {
        base.Awake();
        UIManagerAnimator = GetComponent<Animator>();
        UIManagerAnimator.Play("StartGameCountdown");

        mainMenuButton.onClick.AddListener(() => {
            BackToMainMenu();
        });

        restartButton.onClick.AddListener(() => {
            Restart();
        });
    }

    private void Start() {
        levelTime = GameController.Instance.LevelTime;
    }

    public void UpdateTimerUI(float remainingTime) {
        int minutes = (int)(remainingTime / 60);
        int seconds = (int)(remainingTime % 60);

        timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));

        timerFillImage.fillAmount = remainingTime / levelTime;
    }

    public void UpdateIncomingCoinUI(int incomingCoinCount) {
        if (GameController.Instance.IsGamePlaying) {
            if (incomingCoinCount > 0) {
                UIManagerAnimator.Play("IncomingCoin", 0, 0);
                incomingCoinText.SetText("+" + incomingCoinCount);
            }
            else {
                UIManagerAnimator.Play("IncomingPenalty", 0, 0);
                incomingCoinText.SetText(incomingCoinCount.ToString());
            }
        }

    }

    public void UpdateCurrentCoinUI() {
        currentCoinText.SetText(GameController.Instance.CurrentCoinCount.ToString());
    }

    public void UpdateEndGameScreenScores() {
        correctOrderText.SetText(OrderManager.Instance.CorrectDeliveredOrderCount.ToString());
        wrongOrderText.SetText(OrderManager.Instance.WrongDeliveredOrderCount.ToString());
        missOrderText.SetText(OrderManager.Instance.MissDeliveredOrderCount.ToString());
        scoreText.SetText(GameController.Instance.CurrentCoinCount.ToString());
    }

    public void EnableEndGameScreen() {
        UpdateEndGameScreenScores();
        UIManagerAnimator.Play("EndGameScreen");
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
