using UnityEngine;
using System.Collections;
using System.Diagnostics;

public enum MissionType
{
	ENGINEERING,
	ILLNESS,
	OXYGEN,
    NUM_OF_MISSIONS
}

public enum Difficulty
{
	Easy,   // 1 Minigame per level
	Medium, // 2 Minigames per level
	Hard,   // 3 Minigames per level
	Insane,  // 4 Minigames per level
	Difficulty_Count
}

public class Mission {
	private string level_map;
	public bool mission_lost = false;
	public bool mission_won = false;
	private bool mission_active = false;
    private bool isPaused = false;
	private int array_idx, minigames, minigames_complete = 0, minigames_failed = 0;
	private MissionType mission_type;
	private Difficulty difficulty;
    private float TimePassed = 0;
	private int mission_time = 300;
	private int score = 0;

    //stats


    //end stats

	public int Score() {return score;}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public void Update () {
		if(mission_active && !isPaused)
		{
			if (TimeRemaining() <= 0)
			{
				EndMission();
			}
            TimePassed += Time.deltaTime;
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
	public void Initialise(MissionType type, Difficulty difficulty, string scene, int idx)
	{
		array_idx = idx;
		mission_type = type;
		this.difficulty = difficulty;
        level_map = scene;
	}

	public void StartMission()
	{
		//Peter - find out who to take on the mission
		//display a loading screen
        Application.LoadLevel(level_map);
		mission_active = true;
		UnityEngine.Debug.Log (mission_won);

        switch(mission_type)
        {
        case MissionType.ENGINEERING:
        {
            ++MissionManager.Engineer_Missions;

            break;
        }
        case MissionType.ILLNESS:
        {
            ++MissionManager.Medic_Missions;
            break;
        }
        case MissionType.OXYGEN:
        {
            ++MissionManager.Oxygen_Missions;
            break;
        }
        }

		switch (difficulty) {
		case global::Difficulty.Easy:
			mission_time = MissionManager.EASY_MISSION_LENGTH_SECONDS;
			break;
		case global::Difficulty.Medium:
			mission_time = MissionManager.MEDIUM_MISSION_LENGTH_SECONDS;
			break;
		case global::Difficulty.Hard:
			mission_time = MissionManager.HARD_MISSION_LENGTH_SECONDS;
			break;
		case global::Difficulty.Insane:
			mission_time = MissionManager.INSANE_MISSION_LENGTH_SECONDS;
			break;
		}

		//time_elapsed.Start ();
	}

	public void Begin(int minigame_count)
	{
		UnityEngine.Debug.Log("minigames to complete:" + minigame_count);
		GameObject.FindGameObjectWithTag ("HUD Camera").GetComponent<HUDstats> ().SetNumEvents (minigame_count);
		minigames = minigame_count;
        TimePassed = 0;
	}

	//CHECK THIS FOR ERRORS
	void EndMission()
	{
		if(TimeRemaining() > 0)
		{
			score = (int)((((int)difficulty + 1) * (100 * (int)minigames_complete) - 30 * minigames_failed) * (TimeRemaining() / mission_time));
            if (MissionManager.HiScore < score)
                MissionManager.HiScore = score;

            switch(difficulty)
            {
                case global::Difficulty.Easy:
                {
                    ++MissionManager.Easy_Mission;
                    break;
                }
                case global::Difficulty.Medium:
                {
                    ++MissionManager.Medium_Mission;
                    break;
                }
                case global::Difficulty.Hard:
                {
                    ++MissionManager.Hard_Mission;
                    break;
                }
                case global::Difficulty.Insane:
                {
                    ++MissionManager.Insane_Mission;
                    break;
                }
            }

			//VICTORY SONG
			//VICTORY ANIMATION
			mission_won = true;
            ++MissionManager.Missions_Won;
			MissionManager.totalScore += score;
			//modify global stats
		}
		else
		{
			//DEFEAT SONG
			//DEFEAT ANIMATION
			mission_lost = true;
            ++MissionManager.Missions_Failed;
			//modify global stats
		}

        mission_active = false;
		GameObject.Find("MissionManager").GetComponent<MissionManager>().EndMission(array_idx);
	}

	public void MinigameComplete()
	{
		minigames_complete ++;
		GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().SetEventsComplete(minigames_complete);
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
		UnityEngine.Debug.Log("Checking all objectives complete:" + minigames_complete + "/" + minigames);
		if (minigames == 0) return false;

		if(minigames_complete == minigames)
		{
			mission_won = true;
		}
		UnityEngine.Debug.Log (mission_won);
        return mission_won;
	}

	void Reset()
	{
		mission_lost = false;
		mission_won = false;
		minigames_complete = 0;
        TimePassed = 0;
	}
    public string getLevelName()
    {
        return level_map;
    }

	public float TimeRemaining()
	{
        return (float)(mission_time - TimePassed);
	}

	public MissionType missionType()
	{
		return mission_type;
	}

	public Difficulty Difficulty() {return difficulty;}

	public void setIdx(int idx) {array_idx = idx;}
	public int getIdx() {return array_idx;}

    public void PauseGame(bool pause = true)
    {
        isPaused = pause;
    }

    public void QuitMission()
    {
		TimePassed = mission_time;//MissionManager.MISSION_LENGTH_SECONDS;
    }

    public int miniGamesDone() { return minigames_complete; }
    public int miniGamesfailed() { return minigames_failed; }
    public bool MissionWon() { return mission_won; }
}
