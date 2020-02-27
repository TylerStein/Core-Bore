using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTest : MonoBehaviour
{
    [SerializeField] MasterRotator mr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            mr.spinTo(true);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            mr.spinTo(false);
        }
    }
}
