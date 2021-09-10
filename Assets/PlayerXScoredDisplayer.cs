using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerXScoredDisplayer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txtWhiteOutline, txtBlackOutline, txtPhrase;

    private Animator anim;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        // EventManager.Instance.StartListening(EventManager.Events.OnGameOver, OnGameOver);
        // EventManager.Instance.StartListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);

    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        // EventManager.Instance.StopListening(EventManager.Events.OnGameOver, OnGameOver);
        // EventManager.Instance.StopListening(EventManager.Events.BallIsInPosition, OnBallIsInPosition);
    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnBallHitGround() {
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow() {
        yield return new WaitForSeconds(Data.DelayBeforeShowingScoredText);

        txtPhrase.text = $"Player {GameManager.Instance.GetPlayerWhoJustScored()} Scored!";
        txtBlackOutline.text = txtPhrase.text;
        txtWhiteOutline.text = txtPhrase.text;

        anim.SetTrigger("Show");
        StartCoroutine(WaitAndHide());
    }

    private IEnumerator WaitAndHide() {
        yield return new WaitForSeconds(Data.DelayBeforeHidingScoredText);
        anim.SetTrigger("Hide");
    }

}