using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    private Vector3 start, end;
    void Init(Vector3 a, Vector3 b)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Init(new(1,1,1),new(1,1,1));
    }
}
