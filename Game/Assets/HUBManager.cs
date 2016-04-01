using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUBManager : MonoBehaviour {

	public List<HUBMission> avail_missions;

	void OnGUI()
	{
		//display available missions
		foreach (HUBMission mission in avail_missions) {
			if(GUI.Button(new Rect(mission.pos.x, mission.pos.y, 100, 100), mission.mission.missionType().ToString() + "\n"
			             + mission.mission.Difficulty().ToString() + "\n"
			             + mission.mission.getLevelName()))
			{
				Debug.Log(mission.mission.getIdx());
				GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().StartMission(mission.mission.getIdx());

			}

		}
	}

	public void Initialise(List<Mission> avail_missions)
	{
		this.avail_missions = new List<HUBMission> ();
		foreach (Mission mission in avail_missions) {
			AddMission(mission);
		}
		Debug.Log (this.avail_missions.Count + " " + avail_missions.Count);
	}

	public void AddMission(Mission mission)
	{
		HUBMission temp = new HUBMission ();
		Vector3 temp_pos = GenerateRandomPos ();
		temp.Initialise (mission, temp_pos);
		avail_missions.Add (temp);
		Debug.Log (avail_missions.Count);
	}

	public Vector3 GenerateRandomPos()
	{
		Vector3 temp = new Vector3 ();
		bool running = true;
		bool same_pos_found = false;
		while (running) {
			temp.x = Random.Range(0, Screen.width - 100);
			temp.y = Random.Range(0, Screen.height - 100);
			temp.z = Random.Range(0, 40);
			foreach (HUBMission mission in avail_missions) {
				if(mission.pos.Equals(temp))
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
