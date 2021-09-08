using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private Transform mainCanvas;
    private static GameManager instance;
    public static GameManager Instance { get => instance; private set => instance = value; }
    private int playerJustHitBall = 0, playerOneScore = 0, playerTwoScore = 0, playerServing = 1;
    private bool playerOneWon = false, playerTwoWon = false, isCountingDown = true;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.MatchStarted, OnCountDownOver);
        EventManager.Instance.StartListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
        EventManager.Instance.StartListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        EventManager.Instance.StartListeningWithIntParam(EventManager.Events.PlayerHitTheBall, OnPlayerHitBall);

    }

    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.MatchStarted, OnCountDownOver);
        EventManager.Instance.StopListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
        EventManager.Instance.StopListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        EventManager.Instance.StopListeningWithIntParam(EventManager.Events.PlayerHitTheBall, OnPlayerHitBall);
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);

            Application.targetFrameRate = -1;

        } else {
            Destroy(this);
        }
    }

    internal int GetWinner() {
        return playerOneWon ? 1 : 2;
    }

    IEnumerator Start() {
        yield return new WaitForSeconds(1f); //wait everything set themselves up
        EventManager.Instance.TriggerEvent(EventManager.Events.GameManagerReady);
        EventManager.Instance.TriggerEvent(EventManager.Events.GetReadyForSetBegin);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2)) { }
    }

    private void OnPlayerHitBall(int playerNumber) {
        playerJustHitBall = playerNumber;
    }
    private void OnBallHitGround() {
        if (playerJustHitBall == 1) {
            playerOneScore++;
            playerServing = 1;

            if (playerOneScore >= Data.MatchScoreTarget) {
                playerOneWon = true;
                EventManager.Instance.TriggerEvent(EventManager.Events.MatchEnded);
            }
        } else if (playerJustHitBall == 2) {
            playerTwoScore++;
            playerServing = 2;

            if (playerTwoScore >= Data.MatchScoreTarget) {
                playerTwoWon = true;
                EventManager.Instance.TriggerEvent(EventManager.Events.MatchEnded);
            }
        }

        StartCoroutine(StartNextSetCor());
    }

    private IEnumerator StartNextSetCor() {
        yield return new WaitForSeconds(3f);
        EventManager.Instance.TriggerEvent(EventManager.Events.GetReadyForSetBegin);
    }

    private void OnCountDownOver() {
        isCountingDown = false;
        EventManager.Instance.TriggerEvent(EventManager.Events.StartSet);
    }
    private void OnBallIsInPosition() {
        if (isCountingDown) {
            return;
        }

        EventManager.Instance.TriggerEvent(EventManager.Events.StartSet);
    }
    internal int GetCurrentBallServer() {
        return playerServing;
    }
}