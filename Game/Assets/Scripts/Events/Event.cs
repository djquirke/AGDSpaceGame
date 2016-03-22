using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

	// Use this for initialization
	void Start () {
        tag = "Event";
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Override with your event functionality
	public virtual void Activate()
	{
	}

	// may or may not be needed
	public virtual void Initialise()
	{
	}

	// Called when a minigame is complete to update the mission manager
	public void Success()
	{
        tag = "EventDone";
        GameObject Mission = GameObject.Find("MissionManager");
        if (Mission)
        {
            Mission.GetComponent<MissionManager>().MinigameComplete();
        }
		GameObject.FindGameObjectWithTag("HUD Camera").GetComponent<HUDstats>().event_close = false;
	}

	// Called when a minigame is failed to update the mission manager
	public void Failure()
	{
        GameObject Mission = GameObject.Find("MissionManager");
        if (Mission)
        {
            Mission.GetComponent<MissionManager>().MinigameFailed();
        }
	}
}
