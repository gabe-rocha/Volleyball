using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCrying : IState {
    Player player;

    public PlayerStateCrying(Player player) {
        this.player = player;
    }

    public void OnEnter() {
        Debug.Log("Player State: Crying");
        player.animPlayer.SetBool("isCrying", true);
    }

    public void OnExit() {
        player.animPlayer.SetBool("isCrying", false);
    }

    public IState Tick() {

        return this;
    }
}