using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCheering : IState {
    Player player;
    private float animStartTime, animationLength;

    public PlayerStateCheering(Player player) {
        this.player = player;

        foreach (var animationClip in player.animPlayer.runtimeAnimatorController.animationClips) {
            switch (animationClip.name) {
                case "KayKit Animated Character_Cheer":
                    animationLength = animationClip.length;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnEnter() {
        // Debug.Log("Player State: Cheering");
        player.animPlayer.SetTrigger("Cheer");
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