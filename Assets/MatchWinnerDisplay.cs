using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchWinnerDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txtWhiteOutline, txtBlackOutline, txtPhrase;

    private Animator anim;

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.MatchEnded, OnMatchEnded);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.MatchEnded, OnMatchEnded);

    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnMatchEnded() {
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow() {
        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalText);

        txtPhrase.text = $"Player {GameManager.Instance.GetWinner()} Wins!";
        txtBlackOutline.text = txtPhrase.text;
        txtWhiteOutline.text = txtPhrase.text;

        anim.SetTrigger("Show");
    }
}