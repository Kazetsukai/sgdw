using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MenuOptions : MonoBehaviour {
    
    public void PixelDepth()
    {
        string[] files = StandaloneFileBrowser.OpenFilePanel("Select spritesheet", ".", new[] { new ExtensionFilter("Spritesheet PNG", "png") }, false);

        if (files.Length == 0)
            return;

        var bytes = File.ReadAllBytes(files[0]);

        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(bytes);

        var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

        FindObjectOfType<SpriteRenderer>().sprite = sprite;
    }

}
