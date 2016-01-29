using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour {
	static int MAX_AVAILABLE_MISSIONS = 5;
	static int TIME_BETWEEN_MISSION_SPAWNS = 300;

	private List<Mission> avail_missions;
	private Mission active_mission;
	private float time_since_last_new_mission = 0;

	// Use this for initialization
	void Start () {
		avail_missions = new List<Mission>();
		active_mission = new Mission();
	}
	
	// Update is called once per frame
	void Update () {
		if(avail_missions.Count == 0)
		{
			GenerateMission();
			return;
		}

		time_since_last_new_mission += Time.deltaTime;

		if(avail_missions.Count <= MAX_AVAILABLE_MISSIONS && time_since_last_new_mission > TIME_BETWEEN_MISSION_SPAWNS)
		{
			time_since_last_new_mission = 0;
			GenerateMission();
		}
	}

	void GenerateMission()
	{
		//DEVISE FORMULA FOR CALCULATING WHAT DIFFICULTY LEVEL SHOULD BE BASED ON HOW MANY OF THAT TYPE DONE BEFORE

		int x = Random.Range(0,(int)MissionType.NUM_OF_MISSIONS);
		Mission new_mission = new Mission();
		switch (x)
		{
			case 0:
				new_mission.Initialise(MissionType.ENGINEERING, 1);
				break;
			case 1:
				new_mission.Initialise(MissionType.ILLNESS, 1);
				break;
			case 2:
				new_mission.Initialise(MissionType.OXYGEN, 1);
				break;
			default:
				break;
		}

		avail_missions.Add(new_mission);
	}

	void StartMission()
	{
		//find out which mission is selected from menu
		//active_mission = selected mission
		//avail_missions[selected mission].StartMission(selected mission index);
	}

	public void EndMission(int idx)
	{
		avail_missions.RemoveAt(idx);
	}

	public void MinigameComplete()
	{
		if(active_mission)
		{
			active_mission.MinigameComplete();
		}
	}

	public void MinigameFailed()
	{
		if(active_mission)
		{
			active_mission.MinigameFailed();
		}
	}
}
