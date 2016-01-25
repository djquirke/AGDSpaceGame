using UnityEngine;
using System.Collections;

public class TileGrid : Tile {

	// Use this for initialization
	void Start () {
		flow_exits = new Exits(north, south, east, west);
		Debug.Log (flow_exits.ToString());
	}

	public void MouseClick()
	{
		transform.Rotate(0, 0, -90);
		AdjustExits();
		//Debug.Log("north " + flow_exits.north + ", south " + flow_exits.south + ", east " + flow_exits.east + ", west " + flow_exits.west);
		GameObject.FindGameObjectWithTag("EMG Canvas").GetComponent<BoardManager>().CheckFlow();
	}
}
