using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] Transform playerOneBallServePosition, playerTwoBallServePosition;
    [SerializeField] ParabolaController parabolaController;

    private Vector3 targetPosition;
    private bool alreadyHitGroundThisSet = false;
    private Rigidbody2D rb;
    private CircleCollider2D col;

    public Rigidbody2D Rb { get => rb; set => rb = value; }
    internal bool followingParabola = false;
    private float lastTimeBallPosition;
    private Vector3 lastPosition;

    private void Awake() {
        Rb = GetComponent<Rigidbody2D>();
        Rb.bodyType = RigidbodyType2D.Kinematic;

        col = GetComponent<CircleCollider2D>();

        parabolaController = GetComponent<ParabolaController>();
    }
    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.GetReadyForSetBegin, OnGetReadyForSetBegin);
        EventManager.Instance.StartListening(EventManager.Events.StartSet, OnSetStarted);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.GetReadyForSetBegin, OnGetReadyForSetBegin);
        EventManager.Instance.StopListening(EventManager.Events.StartSet, OnSetStarted);
    }

    private void OnGetReadyForSetBegin() {
        StartCoroutine(MoveToServingPositionCor());
    }
    private void OnSetStarted() {
        Rb.bodyType = RigidbodyType2D.Dynamic;
        Rb.AddForce(Vector2.up * 20f);
    }

    private IEnumerator MoveToServingPositionCor() {

        col.enabled = false;

        if (GameManager.Instance.GetCurrentBallServer() == 1) {
            targetPosition = playerOneBallServePosition.position;
        } else {
            targetPosition = playerTwoBallServePosition.position;
        }

        Rb.velocity = Vector2.zero;
        Rb.angularVelocity = 0f;
        Rb.bodyType = RigidbodyType2D.Kinematic;
        var speed = Mathf.Abs(transform.position.x - targetPosition.x);
        speed = Mathf.Clamp(speed, 5f, 50f);
        while (transform.position.x > targetPosition.x + 0.1f || transform.position.x < targetPosition.x - 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // yield return new WaitForSeconds(1f);
        // transform.position = targetPosition;

        Transform parabolaPoint1 = parabolaController.ParabolaRoot.transform.GetChild(1);

        alreadyHitGroundThisSet = false;
        col.enabled = true;
        EventManager.Instance.TriggerEvent(EventManager.Events.BallIsInPosition);
    }

    private void FixedUpdate() {
        if (followingParabola) {
            if (parabolaController.Animation == false) {
                followingParabola = false;
                //reached the end of the parabola
                // bool leftSide = transform.position.x <= 0f;
                // if (leftSide)
                var direction = transform.position - lastPosition;
                direction.Normalize();
                rb.velocity = direction * 7.5f;
                // else
                // rb.velocity = Vector2.down + Vector2.right;
                // }

                return;
            }
            float step = 0.1f;
            if (Time.time > lastTimeBallPosition + step) {
                lastTimeBallPosition = Time.time;
                lastPosition = transform.position;

                rb.velocity = Vector2.zero;
            }
        }

        Debug.DrawLine(lastPosition, transform.position, Color.magenta);

    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (alreadyHitGroundThisSet) {
            // Rb.velocity = Vector2.zero;
            // Rb.angularVelocity = 0f;
            // Rb.bodyType = RigidbodyType2D.Kinematic;
            return;
        }

        if (other.gameObject.CompareTag("Ground")) {
            // Debug.Log("Ball hit Ground");

            alreadyHitGroundThisSet = true;
            followingParabola = false;
            parabolaController.StopFollow();

            // rb.bodyType = RigidbodyType2D.Dynamic;

            bool leftSide = transform.position.x <= 0f;
            if (leftSide) {
                rb.AddForce(new Vector2(-10, 0));
            } else {
                rb.AddForce(new Vector2(10, 0));
            }

            EventManager.Instance.TriggerEvent(EventManager.Events.BallHitTheGround);
            EventManager.Instance.TriggerEventWithBoolParam(EventManager.Events.BallHitTheGround, leftSide);
        }
    }
}