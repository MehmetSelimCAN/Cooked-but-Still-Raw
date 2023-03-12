using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : Singleton<GameController> {

    [Tooltip("in seconds")]
    [SerializeField] private int levelTime;
    public int LevelTime { get { return levelTime; } }
    private float remainingTime;

    private int currentCoinCount = 0;
    public int CurrentCoinCount { get { return currentCoinCount; } }
    private int orderPenalty = -50;

    private bool isGamePlaying = false;
    public bool IsGamePlaying { get { return isGamePlaying; } }

    public override void Awake() {
        base.Awake();
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame() {
        yield return new WaitForSeconds(3f);
        isGamePlaying = true;

        StartCoroutine(StartTimer());
        OrderManager.Instance.StartGame();
    }

    public void StopGame() {

    }

    public void FinishGame() {
        isGamePlaying = false;
        UIManager.Instance.EnableEndGameScreen();
    }

    private IEnumerator StartTimer() {
        remainingTime = levelTime;
        while (remainingTime > 0) {
            remainingTime -= Time.deltaTime;
            UIManager.Instance.UpdateTimerUI(remainingTime);
            yield return null;
        }

        FinishGame();
    }


    public void CorrectDelivery(OrderUI deliveredOrderUI) {
        int recipePrize = deliveredOrderUI.OrderRecipe.recipePrize;
        int incomingCoinCount = recipePrize;

        currentCoinCount += incomingCoinCount;
        UIManager.Instance.UpdateIncomingCoinUI(incomingCoinCount);
    }

    public void DeliveryPenalty() {
        currentCoinCount += orderPenalty;
        if (currentCoinCount < 0) {
            currentCoinCount = 0;
        }

        UIManager.Instance.UpdateIncomingCoinUI(orderPenalty);
    }
}
