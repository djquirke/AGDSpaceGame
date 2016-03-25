using UnityEngine;
using System.Collections;

public class PipeMinigame : Event {

	public GameObject pipe_game;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Activate ()
	{
		difficulty = GameObject.FindGameObjectWithTag("MissionManager").GetComponent<MissionManager>().ActiveMissionDifficulty();
		GameObject temp = (GameObject)Instantiate(pipe_game);
		temp.transform.position = new Vector3(0, 200, 0);
		temp.GetComponent<PipeGame.BoardManager3D>().Initialise(difficulty, this);
	}
}
