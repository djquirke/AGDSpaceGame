using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUBManager : MonoBehaviour {

	public List<GameObject> avail_missions;
	public GameObject popup_image;
	public Text time_till_spawn = null;

	void Start()
	{
		try {
			List<Mission> missions = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().AvailableMissions();
			if(missions != null)
			{
				Initialise(missions);
			}
		} catch (System.Exception ex) {
			
		}
	}

	void OnGUI()
	{
		if (avail_missions != null) {
			//display available missions
//			foreach (HUBMission mission in avail_missions) {
//				if(GUI.Button(new Rect(mission.pos.x, mission.pos.y, 100, 100), mission.mission.missionType().ToString() + "\n"
//				              + mission.mission.Difficulty().ToString() + "\n"
//				              + mission.mission.getLevelName()))
//				{
//					Debug.Log(mission.mission.getIdx());
//					GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().StartMission(mission.mission.getIdx());
//				}
//				
//			}
			DisplayTimeUntilNewMission ();
		}
	}

	void DisplayTimeUntilNewMission ()
	{
		int t = GameObject.FindGameObjectWithTag ("MissionManager").GetComponent<MissionManager> ().TimeSinceNewMission ();
		int r_t = (MissionManager.TIME_BETWEEN_MISSION_SPAWNS - t) / 1000;

		if(r_t < 0) r_t = 0;

		int mins = r_t / 60;
		int seconds = r_t - (mins * 60);

		if (seconds < 10) {
			if (time_till_spawn) {
				time_till_spawn.text = mins + ":0" + seconds;
			}
			//GUI.TextArea(new Rect(Screen.width / 2 - 50, 50, 100, 20), mins + ":0" + seconds);
		} else {
			if (time_till_spawn) {
				time_till_spawn.text = mins + ":" + seconds;
			}
			//GUI.TextArea(new Rect(Screen.width / 2 - 50, 50, 100, 20), mins + ":" + seconds);
		}

	}

	public void Initialise(List<Mission> avail_missions)
	{
		this.avail_missions = new List<GameObject> ();
		foreach (Mission mission in avail_missions) {
			AddMission(mission);
		}
		Debug.Log (this.avail_missions.Count + " " + avail_missions.Count);
	}

	public void AddMission(Mission mission)
	{
		foreach(GameObject m in avail_missions)
		{
			Debug.Log("passed in idx:" + mission.getIdx() + " saved idx:" + m.GetComponent<LevelPopupScript>().mission.getIdx());
			if(mission.getIdx() == m.GetComponent<LevelPopupScript>().mission.getIdx())
			{
				return;
			}
		}

		Vector3 temp_pos = GenerateRandomPos ();
		GameObject temp = (GameObject)Instantiate(popup_image, temp_pos, new Quaternion());
		Transform canvas_tform = GameObject.FindGameObjectWithTag("HUBCanvas").transform;
		temp.transform.SetParent(canvas_tform);
		temp.transform.localPosition = temp.transform.position;
		LevelPopupScript lps = temp.GetComponent<LevelPopupScript>();
		lps.Initialise(mission);
		avail_missions.Add (temp);
		Debug.Log (avail_missions.Count);
	}

	public Vector3 GenerateRandomPos()
	{
		Vector3 temp = new Vector3 ();
		bool running = true;
		bool same_pos_found = false;
		while (running) {
			temp.x = Random.Range(128 - Screen.width / 2, 3 * Screen.width / 4 - 286);
			temp.y = Random.Range(200 - Screen.height / 2, 3 * Screen.height / 4 - 300);
			temp.z = 400;
			foreach (GameObject mission in avail_missions) {
				if(mission.transform.position.Equals(temp))
				{
					same_pos_found = true;
					break;
				}
			}
			running = same_pos_found;
		}

		return temp;
	}
}
