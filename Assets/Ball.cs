using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] Transform playerOneBallServePosition, playerTwoBallServePosition;

    private Vector3 targetPosition;
    private bool alreadyHitGroundThisSet = false;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
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
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(Vector2.up * 200f);
    }

    private IEnumerator MoveToServingPositionCor() {

        if (GameManager.Instance.GetCurrentBallServer() == 1) {
            targetPosition = playerOneBallServePosition.position;
        } else {
            targetPosition = playerTwoBallServePosition.position;
        }

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        while (transform.position.x > targetPosition.x + 0.1f || transform.position.x < targetPosition.x - 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
            yield return null;
        }

        alreadyHitGroundThisSet = false;
        EventManager.Instance.TriggerEvent(EventManager.Events.BallIsInPosition);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (alreadyHitGroundThisSet) {
            return;
        }

        if (other.gameObject.CompareTag("Ground")) {
            Debug.Log("Ball hit Ground");
            alreadyHitGroundThisSet = true;
            EventManager.Instance.TriggerEvent(EventManager.Events.BallHitTheGround);
        }
    }

}