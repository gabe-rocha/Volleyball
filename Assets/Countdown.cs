using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textCountDown;
    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.GameManagerReady, StartCountDown);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.GameManagerReady, StartCountDown);
    }

    private void StartCountDown() {
        StartCoroutine(StartCountDownCor());
    }

    private IEnumerator StartCountDownCor() {
        int timeToStart = Data.CountdownValue;
        textCountDown.text = timeToStart.ToString();

        while (timeToStart > 1) {
            yield return new WaitForSeconds(1f);
            timeToStart--;
            textCountDown.text = timeToStart.ToString();
        }

        EventManager.Instance.TriggerEvent(EventManager.Events.MatchStarted);
        gameObject.SetActive(false);
    }
}