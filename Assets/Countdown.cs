using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour {

    [SerializeField] private GameObject countDownBG;
    [SerializeField] private TextMeshProUGUI textCountDownBlack;
    [SerializeField] private TextMeshProUGUI textCountDownWhite;
    [SerializeField] private TextMeshProUGUI textCountDownYellow;
    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.StartCountdown, StartCountDown);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.StartCountdown, StartCountDown);
    }

    private void Start() {
        countDownBG.SetActive(false);
    }

    private void StartCountDown() {
        StartCoroutine(StartCountDownCor());
    }

    private IEnumerator StartCountDownCor() {
        SoundManager.Instance.StopBGMIntro();
        countDownBG.SetActive(true);
        int timeToStart = Data.CountdownValue;
        textCountDownBlack.text = timeToStart.ToString();
        textCountDownWhite.text = timeToStart.ToString();
        textCountDownYellow.text = timeToStart.ToString();
        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxCountdownBeep);

        while (timeToStart > 0) {
            yield return new WaitForSeconds(1f);
            timeToStart--;
            textCountDownBlack.text = timeToStart.ToString();
            textCountDownWhite.text = timeToStart.ToString();
            textCountDownYellow.text = timeToStart.ToString();
        }

        EventManager.Instance.TriggerEvent(EventManager.Events.MatchStarted);
        countDownBG.SetActive(false);
    }
}