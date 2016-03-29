using UnityEngine;
using System.Collections;

public class HUDstats : MonoBehaviour {

	public bool event_close = false;
	public float width, height;
	private int event_count, events_done;

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
		DisplayEventInfo ();
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

			if(secs < 10)
				GUI.TextArea (new Rect (width / 2 - 50, 50, 100, 20), mins + ":0" + secs);
			else
				GUI.TextArea (new Rect (width / 2 - 50, 50, 100, 20), mins + ":" + secs);

		}
		catch {
			return;
		}
	}

	private void DisplayEventInfo ()
	{
		GUI.TextArea (new Rect (100, height - 75, 200, 20), "Events Complete: " + events_done + "/" + event_count);
	}

	public void SetNumEvents (int minigame_count)
	{
		event_count = minigame_count;
	}

	public void SetEventsComplete(int count)
	{
		events_done = count;
	}
}
