using UnityEngine;
using System.Collections;

public class HUDstats : MonoBehaviour {

	public bool event_close = false;
	public float width, height;

	// Use this for initialization
	void Start () {
		width = camera.pixelWidth / 2;
		height = 3 * (camera.pixelHeight / 4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(event_close)
		{
			GUI.TextArea(new Rect(width - 50, height, 100, 100), "Press A");
		}
	}
}
