using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class AnimationMaster : MonoBehaviour {

    public GameObject[] toggleables;
    public GameObject animationTarget;
    public GameObject rotationTarget;
    public RenderTexture texture;
    public RenderTexture depthTexture;
    public Shader normalShader;
    public Camera camera;

    float rotation = 180;
    bool rendering;

    AnimationState[] animations = new AnimationState[0];
    Animation animation;

    float scroll;

	// Use this for initialization
	void Start () {
        animation = animationTarget.GetComponent<Animation>();
        animations = animation.OfType<AnimationState>().ToArray();
        camera.targetTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
        scroll += Input.GetAxis("Mouse ScrollWheel") * 500;
        scroll = Mathf.Clamp(scroll, -(animations.Length) * 35, 0);
    }

    void OnGUI()
    {
        var br = new Vector2(Screen.width, Screen.height);

        int y = 20 + (int)scroll;
        foreach (var anim in animations)
        {
            if (GUI.Button(new Rect(10, y, 180, 30), anim.name))
            {
                animationTarget.GetComponent<Animation>().Play(anim.name);
            }
            if (!rendering && GUI.Button(new Rect(190, y, 30, 30), "rec"))
            {
                StartCoroutine(Render(anim.name));
            }
            y += 35;
        }

        y = 20;
        foreach (var toggle in toggleables)
        {
            if (GUI.Button(new Rect(br.x - 110, y, 100, 30), toggle.name))
            {
                toggle.SetActive(!toggle.activeInHierarchy);
            }
            y += 35;
        }


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

    IEnumerator Render(string animName)
    {
        rendering = true;

        var renderers = FindObjectsOfType<MeshRenderer>().ToList();
        var renderers2 = FindObjectsOfType<SkinnedMeshRenderer>().ToList();
        var shaders = renderers.ToDictionary(a => a, a => a.material.shader);
        var shaders2 = renderers2.ToDictionary(a => a, a => a.material.shader);

        animation.Play(animName);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Application.targetFrameRate = 10;
        int i = 0;
        
        while (animation.isPlaying)
        {
            DumpRenderTexture(texture, animName + "_" + rotation + "_" + i.ToString("000") + ".png");

            renderers.ForEach((m) => m.material.shader = normalShader);
            renderers2.ForEach((m) => m.material.shader = normalShader);
            camera.targetTexture = depthTexture;
            camera.Render();
            DumpRenderTexture(depthTexture, animName + "_normal_" + rotation + "_" + i.ToString("000") + ".png");

            renderers.ForEach((m) => m.material.shader = shaders[m]);
            renderers2.ForEach((m) => m.material.shader = shaders2[m]);
            camera.targetTexture = texture;


            i++;
            yield return new WaitForEndOfFrame();
        }

        Application.targetFrameRate = -1;


        // TODO: Later
        // var clip = animation.GetClip(animName);


        Debug.Log("Done");

        rendering = false;
    }

    public static void DumpRenderTexture(RenderTexture rt, string pngOutPath)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }
}
