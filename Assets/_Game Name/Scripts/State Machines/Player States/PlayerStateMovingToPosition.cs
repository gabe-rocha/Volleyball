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
        Debug.Log("Player State: MovingToPosition");

        if (GameManager.Instance.GetCurrentBallServer() == (int)player.playerNumber) {
            Debug.Log($"{player.name} moving to serve position");
            movingToPosition = player.ballServePosition.position;
        } else {
            Debug.Log($"{player.name} moving to mid of court position");
            movingToPosition = player.midOfCourtPosition.position;
        }

        arrived = false;
        player.animPlayer.SetBool("isMoving", true);
    }

    public void OnExit() {
        player.animPlayer.SetBool("isMoving", false);
    }

    public IState Tick() {

        if (arrived) {
            return player.statePlayerIdle;
        } else {
            MoveToPosition();
            return this;
        }
    }

    private void MoveToPosition() {
        if (player.transform.position.x > movingToPosition.x + 0.1f || player.transform.position.x < movingToPosition.x - 0.1f) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, movingToPosition, player.walkSpeed * Time.deltaTime);
        } else {
            arrived = true;
        }
    }
}