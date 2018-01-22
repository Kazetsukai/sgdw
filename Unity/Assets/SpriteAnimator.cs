using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour {

    float timePerFrame = 0.10f; // 10FPS

    public Sprite[] sprites;
    public Sprite[] normalMaps;
    public string[] animationDefinitions;

    Dictionary<string, Sprite[]> animations = new Dictionary<string, Sprite[]>();
    Dictionary<string, Sprite[]> animationNormals = new Dictionary<string, Sprite[]>();

    Sprite[] currentAnimation;
    Sprite[] currentAnimationNormal;
    string currentAnimationName;
    SpriteRenderer spriteRenderer;

    int frame = 0;
    float frameTime = 0;
    Action finishAction;

    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach (var a in animationDefinitions)
        {
            // name:startframe:length
            var split = a.Split(':');

            if (split.Length != 3)
                throw new System.Exception("Invalid animation definition! " + a);

            int start = int.Parse(split[1]);
            int length = int.Parse(split[2]);

            var frames = new Sprite[length];
            var normals = new Sprite[length];

            for (int i = 0; i < length; i++)
            {
                frames[i] = sprites[i + start];
                normals[i] = normalMaps[i + start];
            }

            animations[split[0]] = frames;
            animationNormals[split[0]] = normals;
        }

        currentAnimation = animations.First().Value;
        currentAnimationNormal = animationNormals.First().Value;
    }
	
	void Update () {
        frameTime += Time.deltaTime;
        if (frameTime > timePerFrame)
        {
            frameTime -= timePerFrame;

            bool finished = false;
            while (frame >= currentAnimation.Length)
            {
                frame -= currentAnimation.Length;

                finished = true;
            }
            if (finished && finishAction != null)
            {
                finishAction();
            }

            spriteRenderer.sprite = currentAnimation[frame];
            spriteRenderer.material.SetTexture("_NormalMap", currentAnimationNormal[frame].texture);
            frame++;
        }
	}

    public void SetAnimation(string animName, float frameLength = 0.1f, Action actionOnFinishAnimation = null)
    {
        timePerFrame = frameLength;

        if (!animations.ContainsKey(animName))
        {
            Debug.LogError("Unknown animation: " + animName);
            return;
        }

        var newName = animName.Split('_')[0];

        if (newName != currentAnimationName)
        {
            // Not the same set of animations, so reset to frame 0
            frame = 0;
        }
        currentAnimation = animations[animName];
        currentAnimationNormal = animationNormals[animName];
        currentAnimationName = newName;

        finishAction = actionOnFinishAnimation;
    }
}
