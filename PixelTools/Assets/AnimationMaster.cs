using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimationMaster : MonoBehaviour {

    public GameObject animationTarget;
    public GameObject rotationTarget;
    public RenderTexture texture;

    float rotation = 180;

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
            //if (GUI.Button(new Rect(190, y, 30, 30), "rec"))
            {
              //  animationTarget.GetComponent<Animation>().Play(anim.name);
            }
            y += 35;
        }

        var br = new Vector2(Screen.width, Screen.height);

        GUI.Box(new Rect(br.x - 300, br.y - 300, 256, 256), GUIContent.none);
        GUI.DrawTexture(new Rect(br.x - 300, br.y - 300, 256, 256), texture);

        if (GUI.Button(new Rect(250, 20, 30, 30), "<"))
        {
            rotation += 45;
        }
        if (GUI.Button(new Rect(290, 20, 30, 30), ">"))
        {
            rotation -= 45;
        }

        if (rotation < 0) rotation += 360;
        if (rotation > 359) rotation -= 360;
        rotation = Mathf.RoundToInt(rotation);

        rotationTarget.transform.localRotation = Quaternion.Euler(0, rotation, 0);

        GUI.Label(new Rect(330, 20, 50, 30), rotationTarget.transform.localRotation.eulerAngles.y.ToString());
    }
}
