using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMovingToPosition : IState {
    Player player;
    private bool arrived = false;
    private Vector3 movingToPosition;

    public PlayerStateMovingToPosition(Player player) {
        this.player = player;
    }

    public void OnEnter() {
        // Debug.Log("Player State: MovingToPosition");
        EventManager.Instance.StartListening(EventManager.Events.MathAnswerIsCorrect, OnMathQuestionAnswered);

        if(player.isBallServer && !player.alreadyHitTheBall) {
            Debug.Log($"{player.name} moving to serve position");
            movingToPosition = player.ballServePosition.position;

        } else if(player.isBallServer && player.alreadyHitTheBall) {
            movingToPosition = player.midOfCourtPosition.position;

        } else {
            Debug.Log($"{player.name} moving to mid of court position");
            movingToPosition = player.midOfCourtPosition.position;
        }

        arrived = false;
        player.animPlayer.SetBool("isMoving", true);
    }

    public void OnExit() {
        EventManager.Instance.StopListening(EventManager.Events.MathAnswerIsCorrect, OnMathQuestionAnswered);
        player.animPlayer.SetBool("isMoving", false);
    }

    public IState Tick() {

        // if(arrived && player.isBallServer = false; && player.playerNumber == Player.PlayerNumber.One) {
        // return player.statePlayerAnsweringMath;
        // } else 
        if(arrived && player.answeringQuestion) {
            player.isInplace = true;
            return player.statePlayerAnsweringMath;
        } else if(arrived) {
            return player.statePlayerIdle;
        } else {
            MoveToPosition();
            return this;
        }
    }

    private void MoveToPosition() {
        if(player.transform.position.x > movingToPosition.x + 0.1f || player.transform.position.x < movingToPosition.x - 0.1f) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, movingToPosition, player.walkSpeed * Time.deltaTime);
        } else {
            arrived = true;
        }
    }

    private void OnMathQuestionAnswered() {
        player.isInplace = true;
        player.answeringQuestion = false;
    }
}