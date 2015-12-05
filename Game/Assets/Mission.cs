using UnityEngine;
using System.Collections;

enum MissionType
{
	OXYGEN,
	ENGINEERING,
	ILLNESS
}

public class Mission : MonoBehaviour {

	private float time_remaining = 300;
	private bool mission_lost = false;
	private bool mission_won = false;
	private bool mission_active = false;
	private int array_idx;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(mission_active)
		{
			time_remaining -= Time.deltaTime;
			if(time == 0)
			{
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
		else if(mission_win)
		{
			//display victory screen
			//teleport back to base ship after x seconds or if skip key pressed
		}
	}

	//override this function with specific initialisation requirements for your mission type
	public void Initialise()
	{
	}

	void StartMission(int idx)
	{
		//Peter - find out who to take on the mission
		//display a loading screen
		//Matt - teleport to location/load mission
		mission_active = true;
		array_idx = idx;

	}

	void EndMission()
	{
		if(CheckWin())
		{
			mission_won = true;
			//modify global stats
		}
		else
		{
			mission_lost = true;
			//modify global stats
		}
		GameObject.Find("MissionManager").GetComponent<MissionManager>().EndMission(array_idx);
	}

	//override this function in your mission type
	public bool CheckWin()
	{
	}

	//override this function in your mission type
	public void ResetSpecifics()
	{
	}

	void Reset()
	{
		time_remaining = 300;
		mission_lost = false;
		mission_won = false;
		ResetSpecifics();
	}
}
