using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustRoot : MonoBehaviour {

    public Transform root;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.localPosition = transform.localRotation  * (-root.localPosition);
	}
}
