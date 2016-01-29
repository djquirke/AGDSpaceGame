using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

	//protected Exits flow_exits;
	public bool north, south, east, west, visited;

	// Use this for initialization
	void Start () {
		//flow_exits = new Exits(north, south, east, west);
		Debug.Log(north.ToString() + " " + east.ToString() + " " + south.ToString() + " " + west.ToString());
	}

	public void AdjustExits()
	{
		bool tn = false, te = false, ts = false, tw = false;

		if(north) te = true;
		if(east) ts = true;
		if(south) tw = true;
		if(west) tn = true;

		north = tn;
		east = te;
		south = ts;
		west = tw;
	}

	public void setFlow(bool flow)
	{
		visited = flow;
		Image tile_col = gameObject.GetComponent<Image>();
		if(flow) tile_col.color = new Color(0, 0.5f, 0);
		else tile_col.color = new Color(1, 1, 1);
	}

	public bool isVisited()
	{
		return visited;
	}

	public bool hasExitOppositeTo(Direction dir)
	{
		bool ret = false;

		switch (dir)
		{
		case Direction.EAST:
			ret = west;
			break;
		case Direction.NORTH:
			ret = south;
			break;
		case Direction.SOUTH:
			ret = north;
			break;
		case Direction.WEST:
			ret = east;
			break;
		}

		return ret;
	}
}
