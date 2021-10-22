using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private Transform mainCanvas;
    [SerializeField] private Image splashScreen;
    private static GameManager instance;
    public static GameManager Instance { get => instance; private set => instance = value; }
    internal int playerJustHitBall = 0, playerOneScore = 0, playerTwoScore = 0, playerServing = 1;
    private int playerWhoJustScored;
    private bool playerOneWon = false, playerTwoWon = false, isCountingDown = true;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.MatchStarted, OnCountDownOver);
        EventManager.Instance.StartListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
        EventManager.Instance.StartListeningWithBoolParam(EventManager.Events.BallHitTheGround, OnBallHitGround);
        EventManager.Instance.StartListeningWithIntParam(EventManager.Events.PlayerHitTheBall, OnPlayerHitBall);

    }

    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.MatchStarted, OnCountDownOver);
        EventManager.Instance.StopListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
        EventManager.Instance.StopListeningWithBoolParam(EventManager.Events.BallHitTheGround, OnBallHitGround);
        EventManager.Instance.StopListeningWithIntParam(EventManager.Events.PlayerHitTheBall, OnPlayerHitBall);
    }

    void Awake() {
        if(Instance == null) {
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
        splashScreen.color = Color.white;
        yield return new WaitForSeconds(1f); //wait everything to set themselves up

        // SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxIntro);
        SoundManager.Instance.PlayBGMIntro();

        yield return StartCoroutine(SplashScreenCor());
        EventManager.Instance.TriggerEvent(EventManager.Events.GameManagerReady);

        // splashScreen.CrossFadeAlpha(0, 3, false);
        // yield return new WaitForSeconds(3f);
        EventManager.Instance.TriggerEvent(EventManager.Events.GetReadyForSetBegin);
    }

    private IEnumerator SplashScreenCor() {

        yield return new WaitForSeconds(2f);
        var startTime = Time.time;
        float timeToFade = 1f;
        while (Time.time < startTime + timeToFade) {
            var color = splashScreen.color;
            color.a -= 1f / timeToFade * Time.deltaTime;
            splashScreen.color = color;
            yield return null;
        }
        splashScreen.color = Color.clear;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha2)) { }
    }

    private void OnPlayerHitBall(int playerNumber) {
        playerJustHitBall = playerNumber;
    }
    private void OnBallHitGround(bool leftSide) {

        // if (leftSide)
        // Debug.Log("Ball hit left side");
        // else
        // Debug.Log("Ball hit right side");

        if(playerJustHitBall == 1 && !leftSide || playerJustHitBall == 2 && !leftSide) {
            playerOneScore++;
            playerServing = 1;
            playerWhoJustScored = 1;

        } else if(playerJustHitBall == 2 && leftSide || playerJustHitBall == 1 && leftSide) {
            playerTwoScore++;
            playerServing = 2;
            playerWhoJustScored = 2;
        }

        if(playerOneScore >= Data.MatchScoreTarget) {
            playerOneWon = true;
            EventManager.Instance.TriggerEvent(EventManager.Events.MatchEnded);
        } else if(playerTwoScore >= Data.MatchScoreTarget) {
            playerTwoWon = true;
            EventManager.Instance.TriggerEvent(EventManager.Events.MatchEnded);
        } else {
            StartCoroutine(StartNextSetCor());
        }

    }

    private IEnumerator StartNextSetCor() {
        yield return new WaitForSeconds(4.1f);
        EventManager.Instance.TriggerEvent(EventManager.Events.GetReadyForSetBegin);
    }

    private void OnCountDownOver() {
        isCountingDown = false;
        EventManager.Instance.TriggerEvent(EventManager.Events.StartSet);
        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxWhistle);
    }
    private void OnBallIsInPosition() {
        if(isCountingDown) {
            return;
        }

        EventManager.Instance.TriggerEvent(EventManager.Events.StartSet);
        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxWhistle);
    }
    internal int GetCurrentBallServer() {
        return playerServing;
    }

    internal int GetPlayerWhoJustScored() {
        return playerWhoJustScored;
    }
}