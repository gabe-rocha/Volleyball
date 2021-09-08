using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHittingBall : IState {
    Player player;
    float animationLength;
    private float animStartTime;

    public PlayerStateHittingBall(Player player) {
        this.player = player;
        foreach (var animationClip in player.animPlayer.runtimeAnimatorController.animationClips) {
            switch (animationClip.name) {
                case "KayKit Animated Character_Attack(1h)":
                    animationLength = animationClip.length;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnEnter() {
        Debug.Log("Player State: Hitting The Ball");
        HitBall();
    }

    private void HitBall() {

        player.animPlayer.SetTrigger("Hit Ball");
        animStartTime = Time.time;
        EventManager.Instance.TriggerEventWithIntParam(EventManager.Events.PlayerHitTheBall, (int)player.playerNumber);

        player.ball.attachedRigidbody.velocity = Vector2.zero;
        var top = Data.midTopOfTheScreen;
        var direction = top - player.transform.position;
        direction.Normalize();
        // other.attachedRigidbody.velocity = direction * Mathf.Abs(transform.position.x) * maxPower; //12 = -8 + maxPower
        player.ball.attachedRigidbody.velocity = direction * 11f; //12 = -8 + maxPower
        player.ball.attachedRigidbody.angularVelocity = UnityEngine.Random.Range(-1000f, 1000f);
        // other.attachedRigidbody.AddForce(new Vector2(400, 300));
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