using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwo : Player {

    // [SerializeField] internal PlayerNumber playerNumber = PlayerNumber.Two;
    // [SerializeField] new internal float walkSpeed = 1f;
    // [SerializeField] new internal float walkDeadzone = 0.2f;
    // [SerializeField] new internal Animator animPlayer;
    // [SerializeField] new internal Transform ballServePosition, midOfCourtPosition;

    // internal IState statePlayerIdle, statePlayerWalking, stateMovingToPosition, statePlayerAnsweringMath, statePlayerHittingBall, statePlayerCheering, statePlayerCrying;
    // internal Rigidbody2D rb;
    // internal Collider2D ball;
    // internal bool ballIsHot = false; //means the ball is being played

    private StateMachine playerStateMachine;
    private bool isMySideLeft;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.GetReadyForSetBegin, OnGetReadyForSetBegin);
        EventManager.Instance.StartListeningWithBoolParam(EventManager.Events.BallHitTheGround, OnBallHitTheGround);
        EventManager.Instance.StartListening(EventManager.Events.MatchEnded, OnMatchEnded);
        EventManager.Instance.StartListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);

    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.GetReadyForSetBegin, OnGetReadyForSetBegin);
        EventManager.Instance.StopListeningWithBoolParam(EventManager.Events.BallHitTheGround, OnBallHitTheGround);
        EventManager.Instance.StopListening(EventManager.Events.MatchEnded, OnMatchEnded);
    }

    private void OnMatchEnded() {
        if (GameManager.Instance.GetWinner() == (int)playerNumber) {
            playerStateMachine.SetState(statePlayerCheering);
        } else {
            playerStateMachine.SetState(statePlayerCrying);
        }
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        statePlayerIdle = new PlayerStateIdle(this);
        statePlayerWalking = new PlayerStateWalking(this);
        stateMovingToPosition = new PlayerStateMovingToPosition(this);
        statePlayerHittingBall = new PlayerStateHittingBall(this);
        statePlayerCheering = new PlayerStateCheering(this);
        statePlayerCrying = new PlayerStateCrying(this);
        statePlayerAnsweringMath = new PlayerStateAnsweringMath(this);

        playerStateMachine = new StateMachine();
        playerStateMachine.SetState(statePlayerIdle);

        isMySideLeft = transform.position.x <= 0f;
        Data.playerTwo = this;
        answeringQuestion = false;
    }

    void Update() {
        playerStateMachine.Tick();
    }

    private void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.CompareTag("Ball")) {

            if (ballIsHot == false) {
                return;
            }

            Debug.Log($"{name} hit the ball");
            ball = obj;
            playerStateMachine.SetState(statePlayerHittingBall);
            // EventManager.Instance.TriggerEvent(EventManager.Events.DisplayMathQuestion);
        }
    }

    private void OnGetReadyForSetBegin() {
        playerStateMachine.SetState(stateMovingToPosition);
    }

    private void OnBallHitTheGround(bool ballHitLeftCourt) {
        ballIsHot = false;

        if (GameManager.Instance.playerJustHitBall == (int)playerNumber && ballHitLeftCourt == !isMySideLeft) {
            playerStateMachine.SetState(statePlayerCheering);
        } else if (GameManager.Instance.playerJustHitBall == (int)playerNumber && ballHitLeftCourt == isMySideLeft) {
            playerStateMachine.SetState(statePlayerCrying);
        } else if (GameManager.Instance.playerJustHitBall != (int)playerNumber && ballHitLeftCourt == !isMySideLeft) {
            playerStateMachine.SetState(statePlayerCheering);
        } else if (GameManager.Instance.playerJustHitBall != (int)playerNumber && ballHitLeftCourt == isMySideLeft) {
            playerStateMachine.SetState(statePlayerCrying);
        }
    }
    private void OnBallIsInPosition() {
        ballIsHot = true;
    }
}