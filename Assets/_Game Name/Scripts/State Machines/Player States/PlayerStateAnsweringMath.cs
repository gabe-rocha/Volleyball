using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAnsweringMath : IState {
    Player player;

    public PlayerStateAnsweringMath(Player player) {
        this.player = player;
    }

    public void OnEnter() {
        EventManager.Instance.StartListening(EventManager.Events.MathAnswerIsCorrect, OnMathQuestionAnswered);
        EventManager.Instance.StartListening(EventManager.Events.BallHitTheGround, OnBallHitTheGround);
        Debug.Log("Player State: Answering Math");
        player.answeringQuestion = true;
        // player.animPlayer.SetBool("isMoving", false);
    }

    public void OnExit() {
        EventManager.Instance.StopListening(EventManager.Events.MathAnswerIsCorrect, OnMathQuestionAnswered);
        EventManager.Instance.StopListening(EventManager.Events.BallHitTheGround, OnBallHitTheGround);
        // player.animPlayer.SetBool("isMoving", false);
    }

    public IState Tick() {
        if(player.answeringQuestion && player.isBallServer && !player.isInplace) {
            return player.stateMovingToPosition;
        } else if(player.answeringQuestion) {
            return this;
        } else {
            return player.statePlayerIdle;
        }
    }

    private void OnMathQuestionAnswered() {
        player.answeringQuestion = false;
    }

    private void OnBallHitTheGround() {
        player.answeringQuestion = false;
    }
}