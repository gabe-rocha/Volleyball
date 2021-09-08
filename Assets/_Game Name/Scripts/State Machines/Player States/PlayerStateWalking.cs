using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWalking : IState {
    Player player;
    private float horizontalInput;
    private float vertical;
    private Vector3 dirHor;
    private Vector3 dirVer;
    private Vector3 direction;
    public PlayerStateWalking(Player player) {
        this.player = player;
    }

    public void OnEnter() {
        player.animPlayer.SetBool("isMoving", true);

        Debug.Log("Player State: Walking");
    }

    public void OnExit() {
        player.animPlayer.SetBool("isMoving", false);
    }

    public IState Tick() {
        CheckBounds();
        Move();

        if (horizontalInput >= -player.walkDeadzone && horizontalInput <= player.walkDeadzone) {
            return player.statePlayerIdle;
        }

        return this;
    }

    private void CheckBounds() {

        if (player.playerNumber == Player.PlayerNumber.One) {
            if (player.transform.position.x < player.ballServePosition.position.x) {
                var newPos = player.transform.position;
                newPos.x = player.ballServePosition.position.x;
                player.transform.position = newPos;
            }
        } else if (player.playerNumber == Player.PlayerNumber.Two) {
            if (player.transform.position.x > player.ballServePosition.position.x) {
                var newPos = player.transform.position;
                newPos.x = player.ballServePosition.position.x;
                player.transform.position = newPos;
            }
        }
    }

    private void Move() {
        if (player.playerNumber == Player.PlayerNumber.One) {
            horizontalInput = Input.GetAxisRaw("Horizontal");

        } else if (player.playerNumber == Player.PlayerNumber.Two) {
            if (Input.GetKey(KeyCode.J)) {
                horizontalInput = -1f;
            } else if (Input.GetKey(KeyCode.L)) {
                horizontalInput = 1f;
            } else {
                horizontalInput = 0f;
            }
        }

        direction = Vector3.right * horizontalInput * player.walkSpeed * Time.deltaTime;
        // player.transform.Translate(direction, Space.World);        
        player.rb.velocity = Vector3.right * horizontalInput * player.walkSpeed;
    }
}