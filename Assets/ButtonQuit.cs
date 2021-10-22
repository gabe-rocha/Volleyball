using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuit : MonoBehaviour {
    private Animator anim;

    private void OnEnable() {

        EventManager.Instance.StartListening(EventManager.Events.MatchEnded, OnGameOver);
    }
    private void OnDisable() {
        EventManager.Instance.StartListening(EventManager.Events.MatchEnded, OnGameOver);

    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnGameOver() {
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow() {
        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalButtons);
        anim.SetTrigger("Show");
    }

    // private void Hide(){
    //     anim.SetTrigger("Hide");
    // }

    public void OnButtonQuitPressed() {
        //SceneManager.LoadScene...

        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxButtonQuit);
    }
}