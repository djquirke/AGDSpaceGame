using UnityEngine;
using System.Collections;

public class TileGrid : Tile {

	// Use this for initialization
	void Start () {
		//flow_exits = new Exits(north, south, east, west);
		//Debug.Log (flow_exits.ToString());
	}

	public void MouseClick()
	{
		transform.Rotate(0, 0, -90);
		AdjustExits();
		GameObject.FindGameObjectWithTag("EMG Canvas").GetComponent<BoardManager>().CheckFlow();
	}
}
