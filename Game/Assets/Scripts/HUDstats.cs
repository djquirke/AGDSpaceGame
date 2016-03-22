using UnityEngine;
using System.Collections;

public class HUDstats : MonoBehaviour {

	public bool event_close = false;
	public float width, height;

	// Use this for initialization
	void Start () {
		width = camera.pixelWidth;
		height = camera.pixelHeight;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(event_close)
		{
			GUI.TextArea(new Rect(width / 2 - 50, 3 * height / 4, 100, 100), "Press A");
		}

		DisplayTimeRemaining ();
	}

	private void DisplayTimeRemaining()
	{
		try
		{
			int time_remaining = Mathf.FloorToInt(GameObject.FindGameObjectWithTag ("MissionManager").GetComponent<MissionManager> ().TimeRemaining ());

			if (time_remaining == -1)
				return;
			
			//int time_remaining = MissionManager.MISSION_LENGTH_SECONDS - time_elapsed;
			int mins = Mathf.FloorToInt (time_remaining / 60);
			int secs = time_remaining - mins * 60;
			
			GUI.TextArea (new Rect (width / 2 - 50, 50, 100, 20), mins + ":" + secs);
		}
		catch {
			return;
		}
	}
}
