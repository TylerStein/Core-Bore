using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinnyboi : MonoBehaviour
{
    public float rotateSpeed = 10000.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, Time.deltaTime * rotateSpeed, 0.0f));
    }
}
