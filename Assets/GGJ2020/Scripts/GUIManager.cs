using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : SingletonController<GUIManager>
{
    [SerializeField] public ProgressBarController progressBarController;
    [SerializeField] public DialogBoxController dialogBoxController;
    [SerializeField] public DamageBarController damageBarController;

    private void Start() {
        progressBarController = GetComponentInChildren<ProgressBarController>();
        dialogBoxController = GetComponentInChildren<DialogBoxController>();
        damageBarController = GetComponentInChildren<DamageBarController>();
    }
}
