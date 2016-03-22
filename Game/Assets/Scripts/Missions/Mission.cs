using UnityEngine;
using System.Collections;
using System.Diagnostics;

public enum MissionType
{
	OXYGEN,
	ENGINEERING,
	ILLNESS,
    NUM_OF_MISSIONS
}

public enum Difficulty
{
	Easy,   // 1 Minigame per level
	Medium, // 2 Minigames per level
	Hard,   // 3 Minigames per level
	Insane  // 4 Minigames per level
}

public class Mission {
	private string level_map;
	private Stopwatch time_elapsed;
	private bool mission_lost = false;
	private bool mission_won = false;
	private bool mission_active = false;
	private int array_idx, minigames, minigames_complete = 0, minigames_failed = 0;
	private MissionType mission_type;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public void Update () {
		if(mission_active)
		{
            if (TimeRemaining() <= 0)
			{
				time_elapsed.Stop();
				EndMission();
			}
		}
	}

	void OnGUI()
	{
		if(mission_lost)
		{
			//display losing screen
			//if restart pressed - restart
			//else if back to base pressed - teleport back to base
		}
        else if (mission_won)
		{
			//display victory screen
			//teleport back to base ship after x seconds or if skip key pressed
		}
	}

	// TODO: change minigame count to difficulty
	public void Initialise(MissionType type, int minigame_count, string scene)
	{
		mission_type = type;
		minigames = minigame_count;

        level_map = scene;
        
	}

	public void StartMission()
	{
		//Peter - find out who to take on the mission
		//display a loading screen
        Application.LoadLevel(level_map);
		mission_active = true;
		time_elapsed = new Stopwatch ();
		//time_elapsed.Start ();
	}

	public void Begin()
	{
		time_elapsed.Start ();
	}

	//CHECK THIS FOR ERRORS
	void EndMission()
	{
		if(AllObjectivesComplete() && TimeRemaining() > 0)
		{
			//VICTORY SONG
			//VICTORY ANIMATION
			mission_won = true;
			//modify global stats
		}
		else
		{
			//DEFEAT SONG
			//DEFEAT ANIMATION
			mission_lost = true;
			//modify global stats
		}
		GameObject.Find("MissionManager").GetComponent<MissionManager>().EndMission(array_idx);
	}

	public void MinigameComplete()
	{
		minigames_complete ++;
		if(AllObjectivesComplete())
		{
			EndMission();
		}
	}

	public void MinigameFailed()
	{
		minigames_failed ++;
	}
	
	bool AllObjectivesComplete()
	{
		if(minigames_complete == minigames)
		{
			mission_won = true;

		}
        return mission_won;
	}

	//override this function in your mission type
	public void ResetSpecifics()
	{
	}

	void Reset()
	{
		mission_lost = false;
		mission_won = false;
		minigames_complete = 0;
		ResetSpecifics();
		time_elapsed.Reset();
	}
    public string getLevelName()
    {
        return level_map;
    }

	public float TimeRemaining()
	{
		return (float)(MissionManager.MISSION_LENGTH_SECONDS - time_elapsed.ElapsedMilliseconds / 1000);
	}

	public MissionType missionType()
	{
		return mission_type;
	}
}
