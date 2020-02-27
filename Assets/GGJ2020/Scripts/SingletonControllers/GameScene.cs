using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SingletonController<GameScene> {

    public float BackgroundSpeed;

    private void Start() {
        MapGenSingleton.Inst.GenerateBackground();

        MapGenSingleton.Inst.SetBackgroundPosition(
            new Vector3(0,0,-4.79f), Quaternion.identity);

        /*MapGenSingleton.Inst.SetRockCollider(
            new Vector3(2.56f, 0f, -0.6f), 4f);*/

    }

    private void Update() {
        MapGenSingleton.Inst.StepBackground(BackgroundSpeed);
    }


}
