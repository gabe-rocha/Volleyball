using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathQuestionDisplay : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textQuestionBlackOutline, textQuestionWhiteOutline, textQuestion;
    [SerializeField] private TMP_InputField inputAnswer;

    private string expectedResult;
    private Animator anim;
    private bool isShowing = false;

    private enum Operators {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    private void OnEnable() {
        EventManager.Instance.StartListening(EventManager.Events.DisplayMathQuestion, OnDisplayMathQuestion);
        EventManager.Instance.StartListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        inputAnswer.onSubmit.AddListener(ValidateInput);
    }
    private void OnDisable() {
        EventManager.Instance.StopListening(EventManager.Events.DisplayMathQuestion, OnDisplayMathQuestion);
        EventManager.Instance.StopListening(EventManager.Events.BallHitTheGround, OnBallHitGround);
        inputAnswer.onSubmit.RemoveAllListeners();
    }

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnDisplayMathQuestion() {

        int leftNumber = UnityEngine.Random.Range(1, 10);
        int rightNumber = UnityEngine.Random.Range(1, 10);

        int opID = UnityEngine.Random.Range(0, 4);
        string operation = "";

        switch (opID) {
            case (int)Operators.Addition:
                operation = "+";
                expectedResult = (leftNumber + rightNumber).ToString();
                break;
            case (int)Operators.Subtraction:
                operation = "-";
                expectedResult = (leftNumber - rightNumber).ToString();
                break;
            case (int)Operators.Multiplication:
                operation = "x";
                expectedResult = (leftNumber * rightNumber).ToString();
                break;
            case (int)Operators.Division:
                while (leftNumber % rightNumber != 0) {
                    leftNumber = UnityEngine.Random.Range(0, 21);
                    rightNumber = UnityEngine.Random.Range(1, 11);
                }
                operation = "÷";
                expectedResult = (leftNumber / rightNumber).ToString();
                break;
            default:
                break;
        }

        string question = $"{leftNumber} {operation} {rightNumber} =";

        textQuestion.text = question;
        textQuestionBlackOutline.text = textQuestion.text;
        textQuestionWhiteOutline.text = textQuestion.text;

        inputAnswer.text = "";
        inputAnswer.ActivateInputField();

        Debug.Log($"Expected Result: {expectedResult}");

        anim.SetTrigger("Show");
        isShowing = true;
        SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxMathQuestionShow);
    }

    public void OnButtonOkPressed() {
        ValidateInput(inputAnswer.text);
    }

    private void ValidateInput(string input) {
        if(!isShowing) {
            return;
        }

        if(input == expectedResult) {
            EventManager.Instance.TriggerEvent(EventManager.Events.MathAnswerIsCorrect);
            anim.SetTrigger("Hide");
            isShowing = false;
            SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxMathQuestionCorrect);

        } else {
            SoundManager.Instance.PlaySfx(SoundManager.Instance.sfxWrong);
            inputAnswer.ActivateInputField();
        }
    }

    private void OnBallHitGround() {
        if(isShowing) {
            anim.SetTrigger("Hide");
        }
        isShowing = false;
    }
}