using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : IState {
    Player player;

    private float horizontalInput;
    public PlayerStateIdle(Player player) {
        this.player = player;
    }

    public void OnEnter() {
        Debug.Log("Player State: Idle");
        player.animPlayer.SetBool("isMoving", false);
    }

    public void OnExit() { }

    public IState Tick() {

        if (!player.ballIsHot) {
            return this;
        }

        if (player.playerNumber == Player.PlayerNumber.One) {
            horizontalInput = Input.GetAxisRaw("Horizontal");

        }
#if UNITY_EDITOR
        else if (player.playerNumber == Player.PlayerNumber.Two) {
            if (Input.GetKey(KeyCode.J)) {
                horizontalInput = -1f;
            } else if (Input.GetKey(KeyCode.L)) {
                horizontalInput = 1f;
            } else {
                horizontalInput = 0f;
            }
        }
#endif
        if (horizontalInput < -player.walkDeadzone || horizontalInput > player.walkDeadzone) {
            return player.statePlayerWalking;
        }

        return this;
    }
}