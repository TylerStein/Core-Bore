using System;
using UnityEngine;


public class MapGenTester : MonoBehaviour {

    [SerializeField] private float bgMovespeed;

    private MapGenSingleton mapGen;


    private void Start() {
        MapGenSingleton.Inst.GenerateBackground();
    }

    private void Update() {
        MapGenSingleton.Inst.StepBackground(bgMovespeed);
    }

}

