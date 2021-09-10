using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txtScorePlayer1, txtScorePlayer2;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        EventManager.Instance.StartListening(EventManager.Events.MatchStarted, OnMatchStarted);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
    }

    private void Start() {
        RefreshScores();
    }

    private void OnMatchStarted() {
        RefreshScores();
    }

    private void OnBallHitGround() {
        StartCoroutine(WaitAWhileAndGetScore());
    }

    private IEnumerator WaitAWhileAndGetScore() {
        yield return new WaitForSeconds(0.25f); //wait for the score to be registered

        RefreshScores();
    }
    private void RefreshScores() {
        txtScorePlayer1.text = GameManager.Instance.playerOneScore.ToString();
        txtScorePlayer2.text = GameManager.Instance.playerTwoScore.ToString();
    }
}