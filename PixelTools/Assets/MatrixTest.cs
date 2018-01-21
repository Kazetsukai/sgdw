using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour {

    public bool top;
    public bool front;
    public Matrix4x4 matrix;

    public GameObject follow;

    Camera camera;

    // Use this for initialization
    void Start () {
        camera = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update() {

        var pos = transform.position;
        pos.x = follow.transform.position.x;
        pos.y = follow.transform.position.y + 6;
        pos.z = follow.transform.position.z - 5;
        transform.position = pos;

        camera.ResetWorldToCameraMatrix();
        matrix = camera.worldToCameraMatrix;

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

        camera.worldToCameraMatrix = matrix;
	}
}
