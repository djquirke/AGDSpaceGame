using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
		GameObject.Find("MissionManager").GetComponent<MissionManager>().MinigameComplete();
	}

	// Called when a minigame is failed to update the mission manager
	public void Failure()
	{
		GameObject.Find("MissionManager").GetComponent<MissionManager>().MinigameFailed();
	}
}
