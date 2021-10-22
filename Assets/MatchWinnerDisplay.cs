using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchWinnerDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txtWhiteOutline, txtBlackOutline, txtPhrase;
    [SerializeField] private ParticleSystem confetti;

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
        SoundManager.Instance.PlayBGMIntro();
        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalText);

        var winner = GameManager.Instance.GetWinner();

        if(winner == 1) {
            SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxFanfarre);
        } else {
            SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxGameOver);
        }

        txtPhrase.text = $"Player {winner} Wins!";
        txtBlackOutline.text = txtPhrase.text;
        txtWhiteOutline.text = txtPhrase.text;

        if(winner == 1) {
            SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxFanfarre);
            confetti.Play();
        }

        anim.SetTrigger("Show");
    }
}