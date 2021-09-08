using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStart : MonoBehaviour {

    public void OnButtonStartPressed() {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.TriggerEvent(EventManager.Events.OnStartButtonPressed);
    }
}