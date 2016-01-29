using UnityEngine;
using System.Collections;

public class EngineerMission : Event {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Activate()
	{
		Initialise();
		RunMG();
	}

	//setup pipe minigame
	public override void Initialise()
	{
		//initialise board and populate with tiles
		//jumble up pieces via rotation

	}

	void RunMG()
	{
	}
}
