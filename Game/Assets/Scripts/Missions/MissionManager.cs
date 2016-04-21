using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class MissionManager : MonoBehaviour {
	static int MAX_AVAILABLE_MISSIONS = 5;
	public static int TIME_BETWEEN_MISSION_SPAWNS = 240000;
	static MissionManager instance = null;
	public static int MISSION_LENGTH_SECONDS = 300;
	public static string HUB_WORLD_SCENE = "HUB_World";

    public List<string> Illness_Levels;
    public List<string> Engineer_Levels;
    public List<string> Oxygen_Levels;

    public GameObject Pause_Menu = null;
    private static GameObject Pause_Menu_Instance = null;
    private bool isPaused = false;

	private List<Mission> avail_missions;
	private Mission active_mission;
	private Stopwatch time_since_last_new_mission;

    public GameObject LoadingScreen = null;


    public GameObject StatScreen = null;
    private static GameObject StatScreen_Instance = null;

    //stat data
    public static int Missions_Won = 0;
    public static int Missions_Failed = 0;
    public static int Medic_Missions = 0;
    public static int Engineer_Missions = 0;
    public static int Oxygen_Missions = 0;
    public static int Easy_Mission = 0;
    public static int Medium_Mission = 0;
    public static int Hard_Mission = 0;
    public static int Insane_Mission = 0;
    public static int HiScore = 0;

    //end stat data

	// Use this for initialization
	void Start () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		}

		instance = this;
		avail_missions = new List<Mission> ();
		active_mission = null;
		GenerateMission ();
		time_since_last_new_mission = new Stopwatch ();
		time_since_last_new_mission.Start ();
		DontDestroyOnLoad (this);
		GameObject.FindGameObjectWithTag ("HubManager").GetComponent<HUBManager> ().Initialise (avail_missions);
	}
	
	// Update is called once per frame
	void Update () {
		if (active_mission == null) 
        {
			if (avail_missions.Count == 0) 
            {
				GenerateMission ();
				return;
			}
			
			if (avail_missions.Count < MAX_AVAILABLE_MISSIONS && TimeSinceNewMission() > TIME_BETWEEN_MISSION_SPAWNS) 
            {
				time_since_last_new_mission.Reset();
				time_since_last_new_mission.Start();
				GenerateMission ();
			}

		} else 
        {
			active_mission.Update();
		}

       
	}

	public int TimeSinceNewMission()
	{
		return (int)time_since_last_new_mission.ElapsedMilliseconds;
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
        case 2: // Oxygen
            if (Oxygen_Levels.Count > 0)
            {
                new_mission.Initialise(MissionType.OXYGEN, (Difficulty)y, Oxygen_Levels[Random.Range(0, Oxygen_Levels.Count - 1)], avail_missions.Count);
                avail_missions.Add(new_mission);
                UnityEngine.Debug.Log("Oxygen Created");
            }
            break;
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

        if (StatScreen_Instance)
            Destroy(StatScreen_Instance);

        StatScreen_Instance = (GameObject)Instantiate(StatScreen);

        StartCoroutine(LoadHubAfterTimeOrKey(30));
	}
   IEnumerator LoadHubAfterTimeOrKey(float time)
    {
       //this pause if just to make sure that the Key from the 
       //last minigame doesn't trigger the anykeydown
        yield return new WaitForSeconds(0.1f);

        while (!Input.anyKeyDown && time > 0)
        {
            time -= Time.deltaTime;
            yield return true;
        }
        LoadHubWorld();
    }

    private void LoadHubWorld()
    {
        if (active_mission != null)
        {
            if (StatScreen_Instance)
                Destroy(StatScreen_Instance);

            Application.LoadLevel(MissionManager.HUB_WORLD_SCENE);
            active_mission = null;
            StatScreen_Instance = null;
        }
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

     public void PauseGame()
    {
        PauseGame(!isPaused);
    }

    public void PauseGame(bool pause)
    {
        // display pause menu
        CreatePauseMenu();

        isPaused = pause;

        Pause_Menu_Instance.SetActive(pause);

        var Events = FindObjectsOfType<Event>();

        foreach (var item in Events)
        {
            item.PauseGame(pause);
        }

        FindObjectOfType<Player_Movement>().PauseGame(pause);
        active_mission.PauseGame(pause);

    }
    public void QuitMission()
    {
        active_mission.QuitMission();
        PauseGame(false);
    }

    private void CreatePauseMenu()
    {
        if(Pause_Menu_Instance == null)
        {
            Pause_Menu_Instance = (GameObject)Instantiate(Pause_Menu);

            var ListOfButtons = FindObjectsOfType<UnityEngine.UI.Button>();

            foreach (var item in ListOfButtons)
            {
                if (item.gameObject.name == "Resume")
                {
                    item.onClick.AddListener(PauseGame);
                }
                else if (item.gameObject.name == "Quit")
                {
                    item.onClick.AddListener(QuitMission);
                }
            }

            Pause_Menu_Instance.SetActive(false);
        }
    }

    public Mission GetCurrentMission()
    {
        return active_mission;
    }
}
