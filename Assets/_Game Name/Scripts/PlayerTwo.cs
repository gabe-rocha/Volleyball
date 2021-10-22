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

    [SerializeField] GameObject imgThinking;

    private StateMachine playerStateMachine;
    private bool isMySideLeft;
    private float questionsAnswered = 0;
    private Coroutine coroutineAnswering;

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
        Data.playerTwo = this;
        answeringQuestion = false;

        initialPower = power;
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
            // EventManager.Instance.TriggerEvent(EventManager.Events.DisplayMathQuestion);

            coroutineAnswering = StartCoroutine(FakeAnswering());
        }
    }

    private IEnumerator FakeAnswering() {
        answeringQuestion = true;

        imgThinking.SetActive(true);
        float secondsToThink = UnityEngine.Random.Range(questionsAnswered, questionsAnswered + 2);
        yield return new WaitForSeconds(secondsToThink);

        questionsAnswered++;
        answeringQuestion = false;
        imgThinking.SetActive(false);
        yield return null;
    }

    private void OnGetReadyForSetBegin() {
        isBallServer = GameManager.Instance.GetCurrentBallServer() == (int)playerNumber;
        playerStateMachine.SetState(stateMovingToPosition);

        if(coroutineAnswering != null) {
            StopCoroutine(coroutineAnswering);
        }
        imgThinking.SetActive(false);
        answeringQuestion = false;
    }

    private void OnBallHitTheGround(bool ballHitLeftCourt) {
        ballIsHot = false;
        answeringQuestion = false;
        isBallServer = false;
        alreadyHitTheBall = false;
        isInplace = false;

        power = initialPower;
        questionsAnswered = 0;

        if(GameManager.Instance.playerJustHitBall == (int)playerNumber && ballHitLeftCourt == !isMySideLeft) {
            playerStateMachine.SetState(statePlayerCheering);
        } else if(GameManager.Instance.playerJustHitBall == (int)playerNumber && ballHitLeftCourt == isMySideLeft) {
            playerStateMachine.SetState(statePlayerCrying);

            // if(coroutineAnswering != null) {
            //     StopCoroutine(coroutineAnswering);
            // }
            // imgThinking.SetActive(false);
            // answeringQuestion = false;

        } else if(GameManager.Instance.playerJustHitBall != (int)playerNumber && ballHitLeftCourt == !isMySideLeft) {
            playerStateMachine.SetState(statePlayerCheering);
        } else if(GameManager.Instance.playerJustHitBall != (int)playerNumber && ballHitLeftCourt == isMySideLeft) {
            playerStateMachine.SetState(statePlayerCrying);

            // if(coroutineAnswering != null) {
            //     StopCoroutine(coroutineAnswering);
            // }
            // imgThinking.SetActive(false);
            // answeringQuestion = false;
        }

        if(coroutineAnswering != null) {
            StopCoroutine(coroutineAnswering);
        }
        imgThinking.SetActive(false);
        answeringQuestion = false;
    }
    private void OnBallIsInPosition() {
        ballIsHot = true;
    }
}