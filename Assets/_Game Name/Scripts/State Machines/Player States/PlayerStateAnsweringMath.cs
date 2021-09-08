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
        Debug.Log("Player State: Answering Math");
        player.animPlayer.SetBool("isAnsweringMath", true);
    }

    public void OnExit() {
        player.animPlayer.SetBool("isAnsweringMath", false);
    }

    public IState Tick() {

        return this;
    }
}