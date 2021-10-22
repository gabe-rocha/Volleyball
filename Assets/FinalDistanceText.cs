using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalDistanceText : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txtWhiteOutline, txtBlackOutline, txtDistance;

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
        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalText);
        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxFanfarre);
    }
    private void Hide() { }
}