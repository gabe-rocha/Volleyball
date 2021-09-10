using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCrying : IState {
    Player player;
    private float animStartTime, animationLength;

    public PlayerStateCrying(Player player) {
        this.player = player;

        foreach (var animationClip in player.animPlayer.runtimeAnimatorController.animationClips) {
            switch (animationClip.name) {
                case "KayKit Animated Character_Defeat":
                    animationLength = animationClip.length;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnEnter() {
        // Debug.Log("Player State: Crying");
        player.animPlayer.SetTrigger("Cry");
    }

    public void OnExit() { }

    public IState Tick() {
        //Animation completed?
        if (Time.time >= animStartTime + animationLength) {
            return player.statePlayerIdle;
        }
        return this;
    }
}