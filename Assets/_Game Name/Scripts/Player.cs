using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    internal enum PlayerNumber {
        Zero,
        One,
        Two
    }

    [SerializeField] internal PlayerNumber playerNumber = PlayerNumber.One;
    [SerializeField] internal float walkSpeed = 1f;
    [SerializeField] internal float walkDeadzone = 0.2f;
    [SerializeField] internal Animator animPlayer;
    [SerializeField] internal Transform ballServePosition, midOfCourtPosition;
    [SerializeField] internal PowerMeter powerMeter;

    //
    internal IState statePlayerIdle, statePlayerWalking, stateMovingToPosition, statePlayerAnsweringMath, statePlayerHittingBall, statePlayerCheering, statePlayerCrying;

    private StateMachine playerStateMachine;
    internal Rigidbody2D rb;
    internal Collider2D ball;
    internal bool ballIsHot = false; //means the ball is being played
    internal bool answeringQuestion = false;
    bool isMySideLeft;
    internal float power = 6f, initialPower;
    internal bool isBallServer = false;
    internal bool alreadyHitTheBall = false;
    internal bool isInplace = false;

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
        if(GameManager.Instance.GetWinner() == (int)playerNumber) {
            // playerStateMachine.SetState(statePlayerCheering);
            animPlayer.SetTrigger("Cheer Forever");
        } else {
            // playerStateMachine.SetState(statePlayerCrying);
            animPlayer.SetTrigger("Cry Forever");
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

        initialPower = power;

        Data.playerOne = this;
    }

    void Update() {
        playerStateMachine.Tick();
    }

    private void OnTriggerEnter2D(Collider2D obj) {
        if(obj.gameObject.CompareTag("Ball")) {

            if(ballIsHot == false || answeringQuestion) {
                return;
            }

            Debug.Log($"{name} hit the ball");
            ball = obj;
            playerStateMachine.SetState(statePlayerHittingBall);
            EventManager.Instance.TriggerEvent(EventManager.Events.DisplayMathQuestion);
        }
    }

    private void OnGetReadyForSetBegin() {
        isBallServer = GameManager.Instance.GetCurrentBallServer() == (int)playerNumber;
        playerStateMachine.SetState(stateMovingToPosition);
    }

    private void OnBallHitTheGround(bool ballHitLeftCourt) {
        ballIsHot = false;
        answeringQuestion = false;
        isBallServer = false;
        alreadyHitTheBall = false;
        isInplace = false;

        power = initialPower;

        if(GameManager.Instance.playerJustHitBall == (int)playerNumber && ballHitLeftCourt == !isMySideLeft) {
            playerStateMachine.SetState(statePlayerCheering);
        } else if(GameManager.Instance.playerJustHitBall == (int)playerNumber && ballHitLeftCourt == isMySideLeft) {
            playerStateMachine.SetState(statePlayerCrying);
        } else if(GameManager.Instance.playerJustHitBall != (int)playerNumber && ballHitLeftCourt == !isMySideLeft) {
            playerStateMachine.SetState(statePlayerCheering);
        } else if(GameManager.Instance.playerJustHitBall != (int)playerNumber && ballHitLeftCourt == isMySideLeft) {
            playerStateMachine.SetState(statePlayerCrying);
        }
    }
    private void OnBallIsInPosition() {
        ballIsHot = true;
    }
}