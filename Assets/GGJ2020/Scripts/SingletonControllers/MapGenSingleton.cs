using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


public class MapGenSingleton : SingletonController<MapGenSingleton> {
    [SerializeField] private int rockCount = 30;

    [SerializeField] private Vector3 backgroundSpawnPoint;
    [SerializeField] private Vector3 backgroundDespawnPoint;

    [SerializeField] private Transform backgroundTubePrefab;
    [SerializeField] private Transform[] rockPrefabs;
    [SerializeField] private float backgroundTubeWidth;
    [SerializeField] private float rockCenterSpawnRad;

    //[SerializeField] private Transform rockPusher;


    private Transform[] bgTubes;
    private float[] bgTubesOffsets;
    private Transform parent;
    private float tubePosition;
    private Transform lastRandomizedTube = null;

    private Vector3 backgroundMoveDirection
        => (backgroundDespawnPoint - backgroundSpawnPoint).normalized;


    public void GenerateBackground() {
        float trackDistance = Vector3.Distance(backgroundSpawnPoint, backgroundDespawnPoint);
        int numberOfTube = Mathf.CeilToInt(trackDistance / backgroundTubeWidth);

        bgTubes = new Transform[numberOfTube];
        bgTubesOffsets = new float[numberOfTube];

        for (int i = 0; i < numberOfTube ; i++) {
            float pos = backgroundTubeWidth * i / trackDistance;
            bgTubesOffsets[i] = pos;

            var bgTrans = Instantiate<Transform>(backgroundTubePrefab, parent, true);
            //bgTrans.position = backgroundSpawnPoint - (backgroundMoveDirection * (backgroundTubeWidth * i));
            bgTrans.localPosition = Vector3.LerpUnclamped(backgroundSpawnPoint, backgroundDespawnPoint, pos);
            bgTrans.localRotation = Quaternion.LookRotation(backgroundMoveDirection);
            bgTubes[i] = bgTrans;
            bgTrans.GetComponentInChildren<SexyRockMagic>().SpawnRocks(rockPrefabs, rockCenterSpawnRad, backgroundTubeWidth, rockCount);
        }
    }

    public void StepBackground(float moveSpeed) {
        Transform rightMostTube = null;
        float rightMostValue = float.MaxValue;

        tubePosition += Time.deltaTime * moveSpeed;
        for (int i = 0; i < bgTubes.Length; i++) {
            float pos = (bgTubesOffsets[i] + tubePosition) % 1f;
            bgTubes[i].localPosition
                = Vector3.LerpUnclamped(backgroundSpawnPoint, backgroundDespawnPoint, pos);

            if (pos < rightMostValue) {
                rightMostValue = pos;
                rightMostTube = bgTubes[i];
            }
        }

        if (rightMostTube != lastRandomizedTube) {
            lastRandomizedTube = rightMostTube;
            lastRandomizedTube.GetComponentInChildren<SexyRockMagic>().RandomizePositions(rockCenterSpawnRad, backgroundTubeWidth);
        }

    }

    public void SetBackgroundPosition(Vector3 position, Quaternion rotation) {
        transform.position = position;
        transform.rotation = rotation;
    }

    /*public void SetRockCollider(Vector3 position, float radius) {
        rockPusher.localPosition = position;
        rockPusher.localScale = new Vector3(radius,radius,radius);
    }*/








    protected override void Init() {
        base.Init();
        parent = transform;
    }



}