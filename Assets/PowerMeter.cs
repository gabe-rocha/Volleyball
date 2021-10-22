using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour {
    [SerializeField] private Image imgNeedle;
    [SerializeField] private Transform needleBegin, needleEnd;
    [SerializeField] private float needleSpeed = 2f, minPower = 5f, maxPower = 5f, power;

    private Coroutine needleMoveCor;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
        EventManager.Instance.StartListening(EventManager.Events.DisplayMathQuestion, OnDisplayMathQuestion);
        EventManager.Instance.StartListening(EventManager.Events.MathAnswerIsCorrect, OnMathAnswerIsCorrect);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
        EventManager.Instance.StopListening(EventManager.Events.DisplayMathQuestion, OnDisplayMathQuestion);
        EventManager.Instance.StartListening(EventManager.Events.MathAnswerIsCorrect, OnMathAnswerIsCorrect);
    }

    private void Start() {
        ResetNeedlePosition();
        power = minPower;
    }

    private void OnBallIsInPosition() {
        ResetNeedlePosition();
    }

    private void OnDisplayMathQuestion() {
        ResetNeedlePosition();
        needleMoveCor = StartCoroutine(MoveTowardsEnd());
    }

    private IEnumerator MoveTowardsEnd() {
        while (imgNeedle.transform.position.y > needleEnd.position.y) {
            imgNeedle.transform.position = Vector3.MoveTowards(imgNeedle.transform.position, needleEnd.position, needleSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnMathAnswerIsCorrect() {
        if(needleMoveCor != null) {
            StopCoroutine(needleMoveCor);
        }
    }

    private void ResetNeedlePosition() {
        imgNeedle.transform.position = needleBegin.position;
    }

    internal float GetPower() {

        // var totalDistance = needleBegin.position.y - needleEnd.position.y;
        // var needleDelta = imgNeedle.transform.position.y - needleEnd.position.y;
        // var percentPowerLost = needleDelta / totalDistance;

        power = power > maxPower ? maxPower : power + 1f;
        Debug.Log($"Power: {power}");
        return power;
    }
}