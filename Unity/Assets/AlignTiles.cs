#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlignTiles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[CustomEditor(typeof(AlignTiles))]
public class AlignTilesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Align Tiles"))
        {
            foreach (var sprite in FindObjectsOfType<SpriteRenderer>())
            {
                var alignSize = sprite.bounds.size;

                var pos = sprite.transform.localPosition;
                sprite.transform.localPosition = new Vector3(
                    Mathf.RoundToInt(pos.x / alignSize.x) * alignSize.x,
                    Mathf.RoundToInt(pos.y / alignSize.y) * alignSize.y,
                    pos.z
                );
            }
        }
    }
}

#endif