using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCheering : IState {
    Player player;

    public PlayerStateCheering(Player player) {
        this.player = player;
    }

    public void OnEnter() {
        Debug.Log("Player State: Cheering");

        player.animPlayer.SetBool("isCheering", true);
    }

    public void OnExit() {
        player.animPlayer.SetBool("isCheering", false);
    }

    public IState Tick() {

        return this;
    }
}