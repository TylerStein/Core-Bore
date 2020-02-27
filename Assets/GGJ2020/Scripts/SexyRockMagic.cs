using System;

using UnityEngine;

using Random = UnityEngine.Random;

public class SexyRockMagic : MonoBehaviour {

    private Transform[] rocks;

    public void SpawnRocks(Transform[] rockPrefabs, float radius, float width, int amountToSpawn) {
        rocks = new Transform[amountToSpawn];

        for (int i = 0; i < amountToSpawn; i++) {
            rocks[i] = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], transform);
            rocks[i].localScale = new Vector3(Random.Range(0.25f,1f),Random.Range(0.25f,1f),Random.Range(0.25f,1f));
        }

        RandomizePositions(radius, width);
    }

    public void RandomizePositions(float radius, float width) {
        for (int i = 0; i < rocks.Length; i++) {
            Vector3 pos = Random.insideUnitCircle * radius;
            pos = new Vector3(Random.Range(-(width / 2f), (width / 2f)), pos.x, pos.y < 0 ? -pos.y : pos.y);
            rocks[i].localPosition = pos;
            rocks[i].localRotation = Random.rotation;
        }
    }


}