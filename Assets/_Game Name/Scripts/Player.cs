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

    //
    internal IState statePlayerIdle, statePlayerWalking, stateMovingToPosition, statePlayerAnsweringMath, statePlayerHittingBall, statePlayerCheering, statePlayerCrying;

    private StateMachine playerStateMachine;
    internal Rigidbody2D rb;

    internal Collider2D ball;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.GetReadyForSetBegin, OnGetReadyForSetBegin);
        EventManager.Instance.StartListening(EventManager.Events.MatchEnded, OnMatchEnded);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.GetReadyForSetBegin, OnGetReadyForSetBegin);
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
    }

    void Update() {
        playerStateMachine.Tick();
    }

    private void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.CompareTag("Ball")) {
            Debug.Log($"{name} hit the ball");
            ball = obj;
            playerStateMachine.SetState(statePlayerHittingBall);
        }
    }

    private void OnGetReadyForSetBegin() {
        playerStateMachine.SetState(stateMovingToPosition);
    }
}