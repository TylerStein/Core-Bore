using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogBoxController : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public CanvasGroup dialogGroup;

    public float fadeInTime = 1f;
    public float holdTime = 5f;
    public float fadeOutTime = 1f;

    // 0 - none
    // 1 - fadeIn
    // 2 - hold
    // 3 - fadeOut
    [SerializeField] private int fadeState = 0;
    [SerializeField] private float holdTick = 0f;

    // Start is called before the first frame update
    void Start()
    {
        dialogGroup.alpha = 0;
        dialogText.text = "";

        ShowTextBox("Do a barrel roll", 1.5f, 5f, 1.5f);
    }

    void ShowTextBox(string text, float fadeInTime, float holdTime, float fadeOutTime) {
        this.fadeInTime = fadeInTime;
        this.holdTime = holdTime;
        this.fadeOutTime = fadeOutTime;
        dialogText.text = text;
    }

    private void Update() {
        switch (fadeState) {
            case 1:
                onFadeInState();
                break;
            case 2:
                onHoldState();
                break;
            case 3:
                onFadeOutState();
                break;
        }

        if (fadeState == 1) {
            if (dialogGroup.alpha < 1f) {
                if (fadeInTime > 0f) {
                    dialogGroup.alpha += Time.deltaTime * (1 / fadeInTime);
                } else {
                    dialogGroup.alpha = 1f;
                }
            } else {
                fadeState++;
            }
        }
    }

    private void onFadeInState() {
        if (fadeState != 1) return;

        if (fadeInTime <= 0f) {
            dialogGroup.alpha = 1f;
            fadeState++;
            holdTick = 0;
        } else {
            dialogGroup.alpha += Time.deltaTime * (1 / fadeInTime);
            if (dialogGroup.alpha >= 1f) {
                dialogGroup.alpha = 1f;
                fadeState++;
                holdTick = 0;
            }
        }
    }

    private void onHoldState() {
        if (fadeState != 2) return;

        if (holdTime <= 0f) {
            fadeState++;
        } else {
            holdTick += Time.deltaTime;
            if (holdTick >= holdTime) {
                holdTick = 0;
                fadeState++;
            }
        }
    }

    private void onFadeOutState() {
        if (fadeState != 3) return;

        if (fadeOutTime <= 0f) {
            dialogGroup.alpha = 0f;
            fadeState = 0;
        } else {
            dialogGroup.alpha -= Time.deltaTime * (1 / fadeOutTime);
            if (dialogGroup.alpha <= 0f) {
                dialogGroup.alpha = 0f;
                fadeState = 0;
            }
        }
    }
}
