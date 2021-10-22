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
        player.rb.velocity = Vector2.zero;

        player.alreadyHitTheBall = true;
        HitBall();
    }

    private void HitBall() {

        player.animPlayer.SetTrigger("Hit Ball");
        animStartTime = Time.time;

        // player.ball.attachedRigidbody.velocity = Vector2.zero;
        // var top = Data.midTopOfTheScreen;
        // var direction = top - player.transform.position;
        // direction.Normalize();
        // player.ball.attachedRigidbody.velocity = direction * 5.5f; //5.5f = -8 + maxPower
        // player.ball.attachedRigidbody.angularVelocity = UnityEngine.Random.Range(-1000f, 1000f);

        // b.Rb.bodyType = RigidbodyType2D.Kinematic;

        ParabolaController p = player.ball.gameObject.GetComponent<ParabolaController>();
        p.StopFollow();

        //Set Points
        //Parabola initial point
        var beginPoint = player.ball.gameObject.transform.position;
        p.getPoints()[0].position = beginPoint;

        //Parabola end point
        Vector3 endPoint;
        if(player.playerNumber == Player.PlayerNumber.One) {
            endPoint = Data.playerTwo.transform.position;
            // endPoint.x += UnityEngine.Random.Range(-2f, 2f);
            endPoint.x = Mathf.Clamp(endPoint.x, 1f, 7.5f);

        } else {
            endPoint = Data.playerOne.transform.position;
            // endPoint.x += UnityEngine.Random.Range(-2f, 2f);
            endPoint.x = Mathf.Clamp(endPoint.x, -7.5f, -1f);
        }
        endPoint.y = -4f; //magic number
        p.getPoints()[2].position = endPoint;

        //Parabola mid position
        var midPoint = p.getPoints()[1].position;
        midPoint.x = beginPoint.x + endPoint.x;
        p.getPoints()[1].position = midPoint;

        //Make the ball follow the parabola again
        Ball b = player.ball.GetComponent<Ball>();
        b.followingParabola = true;
        b.Rotate();
        // var speed = player.powerMeter.GetPower();

        // p.RefreshTransforms(50f);

        player.power += 0.75f;
        p.Speed = player.power;
        p.FollowParabola();

        Debug.Log($"Ball Speed: {p.Speed}");
        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxBallHitGround);

        EventManager.Instance.TriggerEventWithIntParam(EventManager.Events.PlayerHitTheBall, (int)player.playerNumber);
    }

    public void OnExit() { }

    public IState Tick() {

        //Animation completed?
        if(Time.time >= animStartTime + animationLength) {

            if(player.playerNumber == Player.PlayerNumber.One) {
                return player.statePlayerAnsweringMath;
            } else if(player.playerNumber == Player.PlayerNumber.Two && player.isBallServer) {
                return player.stateMovingToPosition;
            } else if(player.playerNumber == Player.PlayerNumber.Two) {
                return player.statePlayerIdle;
            }
        }

        return this;
    }
}