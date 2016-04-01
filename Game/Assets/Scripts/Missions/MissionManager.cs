using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class MissionManager : MonoBehaviour {
	static int MAX_AVAILABLE_MISSIONS = 5;
	static int TIME_BETWEEN_MISSION_SPAWNS = 3000;
	static MissionManager instance = null;
	public static int MISSION_LENGTH_SECONDS = 300;
	public static string HUB_WORLD_SCENE = "UI";

    public List<string> Illness_Levels;
    public List<string> Engineer_Levels;
    public List<string> Oxygen_Levels;

	private List<Mission> avail_missions;
	private Mission active_mission;
	private Stopwatch time_since_last_new_mission;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
			avail_missions = new List<Mission> ();
			active_mission = null;
			GenerateMission ();
			time_since_last_new_mission = new Stopwatch ();
			time_since_last_new_mission.Start ();
			DontDestroyOnLoad (this);
			GameObject.FindGameObjectWithTag ("HubManager").GetComponent<HUBManager> ().Initialise (avail_missions);
		} else {
			GameObject.FindGameObjectWithTag ("HubManager").GetComponent<HUBManager> ().Initialise (avail_missions);
			//return this;
		}

		//GameObject.FindGameObjectWithTag("LoadManager").GetComponent<LoadManager>().mManagerReady();
	}
	
	// Update is called once per frame
	void Update () {
		if (active_mission == null) {
			if (avail_missions.Count == 0) {
				GenerateMission ();
				return;
			}
			
			if (avail_missions.Count < MAX_AVAILABLE_MISSIONS && time_since_last_new_mission.ElapsedMilliseconds > TIME_BETWEEN_MISSION_SPAWNS) {
				time_since_last_new_mission.Reset();
				time_since_last_new_mission.Start();
				GenerateMission ();
			}
		} else {
			active_mission.Update();
		}
	}

	void GenerateMission()
	{
		int y = Random.Range(0, (int)Difficulty.Difficulty_Count);
		int x = Random.Range(0,(int)MissionType.NUM_OF_MISSIONS);
		Mission new_mission = new Mission();

		switch (x)
		{
		case 0: // Engineer
        	if (Engineer_Levels.Count > 0)
			{
				UnityEngine.Debug.Log((Difficulty)y);
                new_mission.Initialise(MissionType.ENGINEERING, (Difficulty)y, Engineer_Levels[Random.Range(0, Engineer_Levels.Count - 1)], avail_missions.Count);
				avail_missions.Add(new_mission);
				UnityEngine.Debug.Log("Engineer Created");
            }
			break;
		case 1: // Illness
            if (Illness_Levels.Count > 0)
			{
				UnityEngine.Debug.Log((Difficulty)y);
				new_mission.Initialise(MissionType.ILLNESS, (Difficulty)y, Illness_Levels[Random.Range(0, Illness_Levels.Count - 1)], avail_missions.Count);
                avail_missions.Add(new_mission);
				UnityEngine.Debug.Log("Illness Created");
            }
			break;
//		case 2: // Oxygen
//            if (Oxygen_Levels.Count > 0)
//            {
//				new_mission.Initialise(MissionType.OXYGEN, (Difficulty)y, Oxygen_Levels[Random.Range(0, Oxygen_Levels.Count - 1)], avail_missions.Count);
//				avail_missions.Add(new_mission);
//				UnityEngine.Debug.Log("Oxygen Created");
//            }
//			break;
		default:
			break;
		}

		try {
			GameObject.FindGameObjectWithTag ("HubManager").GetComponent<HUBManager> ().AddMission (new_mission);
		} catch (System.Exception ex) {
			UnityEngine.Debug.Log("Hub Manager not available");
		}

	}

	public void StartMission(int idx)
	{
		//find out which mission is selected from menu
		if (active_mission == null)
			active_mission = new Mission ();

        active_mission = avail_missions[idx]; //TODO: change to index of selected mission
		active_mission.StartMission();
	}

	public void EndMission(int idx)
	{
		avail_missions.RemoveAt(idx);
		for (int i = 0; i < avail_missions.Count; i++) {
			avail_missions[i].setIdx(i);
		}
		active_mission = null;
	}

	public void MinigameComplete()
	{
        if (active_mission != null)
		{
			active_mission.MinigameComplete();
		}
	}

	public void MinigameFailed()
	{
        if (active_mission != null)
		{
			active_mission.MinigameFailed();
		}
	}

	public float TimeRemaining()
	{
		if(active_mission != null)
		{
			return active_mission.TimeRemaining();
		}

		return -1;
	}

	public void LevelLoaded(int minigame_count)
	{
		if(active_mission != null)
			active_mission.Begin (minigame_count);
	}

	public MissionType ActiveMissionType()
	{
		if(active_mission != null)
			return active_mission.missionType ();
		return MissionType.NUM_OF_MISSIONS;
	}

	public Difficulty ActiveMissionDifficulty()
	{
		if(active_mission != null)
			return active_mission.Difficulty ();
		return Difficulty.Difficulty_Count;
	}

	public List<Mission> AvailableMissions()
	{
		return avail_missions;
	}
}
