using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour {

    public bool top;
    public bool front;

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update() {

        var matrix = Camera.main.worldToCameraMatrix;

        matrix.m11 = 0.7071068f;
        matrix.m12 = 0.7071068f;

        if (top)
        { 
            matrix.m12 = 1;
        }

        if (front)
        {
            matrix.m11 = 1;
        }
        
        Camera.main.worldToCameraMatrix = matrix;
	}
}
