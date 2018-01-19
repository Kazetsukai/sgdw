using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	private float defaultSize;

	private Camera localCamera;
    

	public JankMode pixelMode = JankMode.Dejank;

	// Use this for initialization
	void Start()
	{
		localCamera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			if (pixelMode == JankMode.Dejank)
				pixelMode = JankMode.FullJank;
			else if (pixelMode == JankMode.FullJank)
				pixelMode = JankMode.Dejank;
		}
        

		localCamera.orthographicSize = 5;

        var size = new Vector2(localCamera.pixelWidth, localCamera.pixelHeight);
        var zoomFactor = 5;
        
		var pixelXOffset = size.x % 2 == 0 ? 0 : 0.5f;
		var pixelYOffset = size.y % 2 == 0 ? 0 : 0.5f;
		var snapSize = ((int)size.y / (zoomFactor)) / 2f;
		var snapPosition = new Vector3((int)(transform.position.x), (int)((transform.position.y)), -20f);

        localCamera.orthographicSize = snapSize;

        if (pixelMode == JankMode.Dejank)
		{
			transform.localPosition = snapPosition;
		}
	}

	public enum JankMode
	{
		FullJank,
		Dejank
	}
}
