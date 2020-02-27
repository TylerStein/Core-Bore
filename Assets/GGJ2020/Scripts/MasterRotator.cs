using System;
using System.Collections;
using UnityEngine;

/// <summary>
///BEHOLD
///THE LAZIEST ROTATION SCRIPT IN THE WORLD
///SIMPLY SUPPLY IT A PREFAB
///AND AWAAAAY WE GO! 
/// </summary>
public class MasterRotator : MonoBehaviour
{
    public MasterRotator Instance { get; private set; }

    [SerializeField] Transform ship;
    float t = 0.0f;

    private Quaternion targetRotation = Quaternion.identity;



    void Start() {
        if(Instance == null) {
            Instance = this;
        }
    }


    private void Update() {
        ship.localRotation = Quaternion.Slerp(ship.localRotation, targetRotation, Time.deltaTime * 10f);
    }



    public void spinTo(bool direction) {
        targetRotation *= Quaternion.Euler(0,30f * (direction ? 1f : -1f), 0);
    }




}
