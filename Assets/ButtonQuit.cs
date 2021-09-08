using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuit : MonoBehaviour {
    private Animator anim;

    private void OnEnable() { }
    private void OnDisable() { }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Show() {
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow() {
        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalButtons);
        anim.SetTrigger("Show");
    }

    // private void Hide(){
    //     anim.SetTrigger("Hide");
    // }

    public void OnButtonQuitPressed() { }
}