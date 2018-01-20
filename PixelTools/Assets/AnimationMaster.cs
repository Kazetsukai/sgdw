using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationMaster : MonoBehaviour {

    public GameObject animationTarget;
    public RenderTexture texture;

    AnimationState[] animations = new AnimationState[0];

    float scroll;

	// Use this for initialization
	void Start () {
        animations = animationTarget.GetComponent<Animation>().OfType<AnimationState>().ToArray();
	}
	
	// Update is called once per frame
	void Update () {
        scroll += Input.GetAxis("Mouse ScrollWheel") * 500;
        scroll = Mathf.Clamp(scroll, -(animations.Length) * 35, 0);
    }

    void OnGUI()
    {
        int y = 20 + (int)scroll;
        foreach (var anim in animations)
        {
            if (GUI.Button(new Rect(10, y, 180, 30), anim.name))
            {
                animationTarget.GetComponent<Animation>().Play(anim.name);
            }
            y += 35;
        }

        GUI.DrawTexture(new Rect(700, 500, 256, 256), texture);
    }
}
